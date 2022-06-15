using GameEngine;
using GameEngine.Entities;
using GameEngine.Services;
using Repository;

namespace UI
{
    /// <summary>
    /// The class contains all logic layer to start a game from User Interface.
    /// </summary>
    public class GameManager : IGameManager
    {
        private readonly IUserInterface _userInterface;
        private readonly IWindow _window;
        
        private readonly IAnimalActions _animalActions;
        private Board _board;

        List<Animal> animals = new List<Animal>();

        /// <summary>
        /// The class contains all logic layer to start a game from User Interface.
        /// </summary>
        /// <param name="userInterface">User Interface.</param>
        /// <param name="window">Window.</param>
        /// <param name="animalActions">Animal Action.</param>
        public GameManager(IUserInterface userInterface, 
                            IWindow window, 
                            IAnimalActions animalActions)
        {
            _userInterface = userInterface;
            _window = window;
            _animalActions = animalActions;
            _board = new Board();
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public void RunApplication()
        {
            _window.WindowConfiguration();

            _window.ClearConsole();
            _userInterface.DrawBorders(ConstantsRepository.BorderWidth, ConstantsRepository.BorderHeight);

            _userInterface.DrawBoard(_board);

            _window.SetCursorPosition(0, 20);
            _userInterface.ShowMenuButtons();

            RunGame();
        }

        /// <summary>
        /// Runs the game every second, while user press Q or Escape button.
        /// </summary>
        private void RunGame()
        {
            bool isGameOnGoing = true;
            while (isGameOnGoing)
            {
                _board.FillBoardWithAnimals(animals);

                _userInterface.DrawBoard(_board);
                _board.ChangeAnimalWithEmptyCell(animals);

                _animalActions.Move(_board, animals);
                _animalActions.ClearBoard(animals);
                Thread.Sleep(ConstantsRepository.ThreadSleep);
                ConsoleKey? consoleKey = _userInterface.GetInputKey();
                
                switch (consoleKey)
                {
                    case ConsoleKey.A:
                        _animalActions.AddAnimal(new Antilope(), animals, _board);
                        break;
                    case ConsoleKey.L:
                        _animalActions.AddAnimal(new Lion(), animals, _board);
                        break;
                }

                isGameOnGoing = (consoleKey != ConsoleKey.Q && consoleKey != ConsoleKey.Escape);
            }
        }
    }
}