using GameEngine;
using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Services.Managers;
using Repository;
using System.Reflection;
using System.Runtime.Loader;

namespace UI
{
    /// <summary>
    /// The class contains all logic layer to start a game from User Interface.
    /// </summary>
    public class GameManager : IGameManager
    {
        private readonly IUserInterface _userInterface;
        private readonly IWindow _window;
        private readonly IAdditionManager _additionManager;
        private readonly IDeletionManager _deletionManager;
        private readonly IPairManager _pairManager;
        private readonly IMovementManager _movementManager;

        private System.Timers.Timer _timer;
        private Board _board;
        private List<IItem> _gameItems;
        private List<Pair> _pairs;
        private List<IItem> _childrenGameItems;

        private Dictionary<ConsoleKey, IItem> _gameItemsInfo = new Dictionary<ConsoleKey, IItem>();

        /// <summary>
        /// The class contains all logic layer to start a game from User Interface.
        /// </summary>
        /// <param name="userInterface">User Interface.</param>
        /// <param name="window">Window.</param>
        /// <param name="additionManager">Addition manager.</param>
        /// <param name="deletionManager">Deletion manager.</param>
        /// <param name="pairManager">Pair manager.</param>
        /// <param name="movementManager">Movement manager.</param>
        public GameManager(
            IUserInterface userInterface,
            IWindow window,
            IAdditionManager additionManager,
            IDeletionManager deletionManager,
            IPairManager pairManager,
            IMovementManager movementManager)
        {
            _userInterface = userInterface;
            _window = window;
            _additionManager = additionManager;
            _deletionManager = deletionManager;
            _pairManager = pairManager;
            _movementManager = movementManager;
            _board = new Board();
            _gameItems = new List<IItem>();
            _pairs = new List<Pair>();
            _childrenGameItems = new List<IItem>();
            LoadInstancesOfDllChildrenClasses();
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

            foreach (var info in _gameItemsInfo)
            {
                _window.SetFontColor(info.Value.Color);
                _userInterface.ShowMessage($"Press {info.Key} - to add {info.Value.Species};");
                _window.ResetFontColor();
            }

            Console.BackgroundColor = ConsoleColor.White;
            _window.SetFontColor(ConsoleColor.Black);
            _userInterface.ShowMessage(ConstantsRepository.ExitButtonDescription);
            _window.ResetFontColor();

            SetTimer();
                        
            bool isGameOnGoing = true;
            while (isGameOnGoing)
            {
                ConsoleKey? consoleKey = _userInterface.GetInputKey();

                _additionManager.ProcessAddNewItem(consoleKey, _gameItemsInfo, _gameItems, _board);

                isGameOnGoing = (consoleKey != ConsoleKey.Q && consoleKey != ConsoleKey.Escape);
            }

            _timer.Stop();
            _timer.Dispose();
        }

        /// <summary>
        /// Sets timer.
        /// </summary>
        private void SetTimer()
        {
            _timer = new System.Timers.Timer(ConstantsRepository.TimeInterval);
            _timer.Elapsed += (source, e) => RunGame();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        /// <summary>
        /// Runs the game every second, while user press Q or Escape button.
        /// </summary>
        private void RunGame()
        {
            Console.CursorVisible = false;

            _board.FillBoardWithItems(_gameItems);

            _userInterface.DrawBoard(_board);
            _board.PrepareBoard(_gameItems);
            
            _movementManager.Act(_gameItems, _board);

            _pairManager.RemoveNotExistingPairs(_pairs);
            _pairManager.CheckPairForExistence(_pairs, _board, _gameItems, _childrenGameItems);
            _pairManager.AddPairToList(_pairManager.CreatePair(_gameItems), _pairs);
                
            _additionManager.ProcessChildrenItems(_gameItems, _childrenGameItems);

            _deletionManager.RemoveInactiveItems(_gameItems);
        }

        /// <summary>
        /// Loads instances of the DLLs' children classes. 
        /// </summary>
        private void LoadInstancesOfDllChildrenClasses()
        {
            List<IItem> boardItems = LoadBoardItems().ToList();

            foreach (var boardItem in boardItems)
            {
                _gameItemsInfo.Add(boardItem.Letter, boardItem);                
            }
        }

        /// <summary>
        /// Load DLL board items from Plugins directory.
        /// </summary>
        /// <returns>Board items.</returns>
        private IEnumerable<IItem> LoadBoardItems()
        {
            List<IItem> boardItems = new List<IItem>();

            foreach (var dll in Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}/{ConstantsRepository.PluginsFolder}", "*.dll"))
            {
                AssemblyLoadContext assemblyLoadContext = new AssemblyLoadContext(dll);
                Assembly assembly = Assembly.LoadFrom(dll);
                boardItems = assembly.ExportedTypes
                    .Where(t => typeof(IItem).IsAssignableFrom(t))
                    .Select(plugin => (IItem)Activator.CreateInstance(plugin)).ToList();
            }

            return boardItems;
        }
    }
}