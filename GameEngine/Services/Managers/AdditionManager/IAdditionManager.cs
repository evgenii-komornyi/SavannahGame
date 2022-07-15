using GameEngine.Entities;
using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains addition methods for the game items list.
    /// </summary>
    public interface IAdditionManager
    {
        /// <summary>
        /// Adds new item to the list of the items by user's input on the current board. 
        /// </summary>
        /// <param name="consoleKey">Console key. Pressed button on keyboard.</param>
        /// <param name="gameItemsInfo">Game items info.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="board">Board.</param>
        void ProcessAddNewItem(ConsoleKey? consoleKey, Dictionary<ConsoleKey, GameItemsInfo> gameItemsInfo, List<IItem> gameItems, Board board);

        /// <summary>
        /// Adds borned children to common game items' list.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <param name="childrenItems">Children.</param>
        void ProcessChildrenItems(List<IItem> gameItems, List<IItem> childrenItems);
    }
}