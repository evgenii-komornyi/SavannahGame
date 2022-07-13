using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains controll of the game items list.
    /// </summary>
    public interface IItemManager
    {
        /// <summary>
        /// Adds new item to the list of the items on the current board. 
        /// </summary>
        /// <param name="newGameItem">New game item.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="board">Board.</param>
        void AddItem(IItem newGameItem, List<IItem> gameItems, Board board);

        /// <summary>
        /// Adds borned children to common game items' list.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <param name="childrenItems">Children.</param>
        void ProcessChildrenItems(List<IItem> gameItems, List<IItem> childrenItems);

        /// <summary>
        /// Removes all inactive items from board. 
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        void RemoveInactiveItems(List<IItem> gameItems);
    }
}