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
        private readonly IPairManager _pairManager;
        
        private Board _board;
        private List<Animal> _animals;
        private List<Pair> _pairs;
        private List<Animal> _children;

        /// <summary>
        /// The class contains all logic layer to start a game from User Interface.
        /// </summary>
        /// <param name="userInterface">User Interface.</param>
        /// <param name="window">Window.</param>
        /// <param name="animalActions">Animal Action.</param>
        /// <param name="pairManager">Pair manager.</param>
        public GameManager(
            IUserInterface userInterface,
            IWindow window,
            IAnimalActions animalActions,
            IPairManager pairManager)
        {
            _userInterface = userInterface;
            _window = window;
            _animalActions = animalActions;
            _pairManager = pairManager;
            _board = new Board();
            _animals = new List<Animal>();
            _pairs = new List<Pair>();
            _children = new List<Animal>();
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public void RunApplication()
        {
            Console.Title = ConstantsRepository.GameTitle;
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
                Console.CursorVisible = false;
                Pair? currentPair = _pairManager.Pair;

                _board.FillBoardWithAnimals(_animals);

                _userInterface.DrawBoard(_board);
                _board.RemoveAnimalFromBoard(_animals);

                _animalActions.Move(_board, _animals);

                _pairManager.RemoveNonExistsPairs(_pairs);

                _pairManager.CheckPairForExistence(_pairs, _board, _animals, _children);

                _pairManager.CreatePair(_animals);

                if (currentPair != null)
                {
                    _pairs.Add(currentPair);
                }

                _animalActions.AddChildren(_animals, _children);
                _children.Clear();

                _animalActions.RemoveDeadAnimals(_animals);
                Thread.Sleep(ConstantsRepository.ThreadSleep);
                ConsoleKey? consoleKey = _userInterface.GetInputKey();

                switch (consoleKey)
                {
                    case ConsoleKey.A:
                        _animalActions.AddAnimal(new Antilope(), _animals, _board);
                        break;
                    case ConsoleKey.L:
                        _animalActions.AddAnimal(new Lion(), _animals, _board);
                        break;
                }

                isGameOnGoing = (consoleKey != ConsoleKey.Q && consoleKey != ConsoleKey.Escape);
            }
        }
    }
}