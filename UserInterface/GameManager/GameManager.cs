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
        private readonly IObjectManager _objectManager;
        private readonly IPairManager _pairManager;
        private readonly IMovementManager _movementManager;

        private Board _board;
        private List<IItem> _gameObjects;
        private List<Pair> _pairs;
        private List<IItem> _childrenGameObjects;

        private Dictionary<ConsoleKey, GameObjectsInfo> _gameObjectsInfo = new Dictionary<ConsoleKey, GameObjectsInfo>();

        /// <summary>
        /// The class contains all logic layer to start a game from User Interface.
        /// </summary>
        /// <param name="userInterface">User Interface.</param>
        /// <param name="window">Window.</param>
        /// <param name="objectManager">Object manager.</param>
        /// <param name="pairManager">Pair manager.</param>
        /// <param name="movementManager">Movement manager.</param>
        public GameManager(
            IUserInterface userInterface,
            IWindow window,
            IObjectManager objectManager,
            IPairManager pairManager,
            IMovementManager movementManager)
        {
            _userInterface = userInterface;
            _window = window;
            _objectManager = objectManager;
            _pairManager = pairManager;
            _movementManager = movementManager;
            _board = new Board();
            _gameObjects = new List<IItem>();
            _pairs = new List<Pair>();
            _childrenGameObjects = new List<IItem>();
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

            foreach (var info in _gameObjectsInfo)
            {
                _window.SetFontColor(info.Value.Color);
                _userInterface.ShowMessage($"Press {info.Key} - to add {info.Value.Specie};");
                _window.ResetFontColor();
            }

            Console.BackgroundColor = ConsoleColor.White;
            _window.SetFontColor(ConsoleColor.Black);
            _userInterface.ShowMessage(ConstantsRepository.ExitButtonDescription);
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

                _board.FillBoardWithObjects(_gameObjects);

                _userInterface.DrawBoard(_board);
                _board.RemoveGameObjectFromBoard(_gameObjects);
                
                _movementManager.Act(_gameObjects, _board);

                _pairManager.RemoveNonExistsPairs(_pairs);

                _pairManager.CheckPairForExistence(_pairs, _board, _gameObjects, _childrenGameObjects);

                _pairManager.CreatePair(_gameObjects);

                if (currentPair != null)
                {
                    _pairs.Add(currentPair);
                }

                _objectManager.AddChildrenObject(_gameObjects, _childrenGameObjects);
                _childrenGameObjects.Clear();

                _objectManager.RemoveInactiveObjects(_gameObjects);
                Thread.Sleep(ConstantsRepository.ThreadSleep);
                ConsoleKey? consoleKey = _userInterface.GetInputKey();
            
                foreach (var info in _gameObjectsInfo)
                {
                    if (consoleKey == info.Key)
                    {
                        _objectManager.AddObject((IItem)Activator.CreateInstance(info.Value.Type), _gameObjects, _board);
                    }
                }    

                isGameOnGoing = (consoleKey != ConsoleKey.Q && consoleKey != ConsoleKey.Escape);
            }
        }

        /// <summary>
        /// Loads instances of the DLLs' children classes. 
        /// </summary>
        private void LoadInstancesOfDllChildrenClasses()
        {
            List<IItem> plugins = LoadPlugins().ToList();

            foreach (var plugin in plugins)
            {
                GameObjectsInfo info = new GameObjectsInfo
                {
                    Specie = plugin.Specie,
                    Color = plugin.Color,
                    Type = plugin.GetType()
                };
                _gameObjectsInfo.Add(plugin.Letter, info);                
            }
        }

        /// <summary>
        /// Load DLL plugins from Plugins directory.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IItem> LoadPlugins()
        {
            List<IItem> result = new List<IItem>();

            foreach (var dll in Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}/{ConstantsRepository.PluginsFolder}", "*.dll"))
            {
                AssemblyLoadContext assemblyLoadContext = new AssemblyLoadContext(dll);
                Assembly assembly = Assembly.LoadFrom(dll);
                var plugins = assembly.ExportedTypes
                    .Where(t => typeof(IItem).IsAssignableFrom(t)).ToList();

                for (int i = 0; i < plugins.Count; i++)
                {
                    var plugin = plugins[i];
                    result.Add((IItem)Activator.CreateInstance(plugin));
                }
            }

            return result;
        }
    }
}