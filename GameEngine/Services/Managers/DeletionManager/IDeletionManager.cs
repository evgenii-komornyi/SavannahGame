using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains deletion method from the game items list.
    /// </summary>
    public interface IDeletionManager
    {
        /// <summary>
        /// Removes all inactive items from board. 
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        void RemoveInactiveItems(List<IItem> gameItems);
    }
}
