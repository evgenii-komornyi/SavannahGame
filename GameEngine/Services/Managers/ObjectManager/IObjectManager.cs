using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains controll of the game objects list.
    /// </summary>
    public interface IObjectManager
    {
        /// <summary>
        /// Adds new object to the list of the objects on the current board. 
        /// </summary>
        /// <param name="newGameObject">New game object.</param>
        /// <param name="animals">Game objects.</param>
        /// <param name="board">Board.</param>
        void AddObject(IItem newGameObject, List<IItem> gameObjects, Board board);

        /// <summary>
        /// Adds borned children to common gam objects' list.
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="childrenObjects">Children.</param>
        void ProcessChildrenObjects(List<IItem> gameObjects, List<IItem> childrenObjects);

        /// <summary>
        /// Removes all inactive objects from board. 
        /// </summary>
        /// <param name="gameObjects">Game objects.</param>
        void RemoveInactiveObjects(List<IItem> gameObjects);
    }
}