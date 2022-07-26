using GameEngine.Helpers;
using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    public class DeletionManager : IDeletionManager
    {
        /// <summary>
        /// Removes all inactive items from board. 
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        public void RemoveInactiveItems(List<IItem> gameItems)
        {
            IEnumerable<IItem> inactiveItems = CheckForActive(gameItems);
            gameItems.RemoveAll(currentItem => inactiveItems.Contains(currentItem));
        }

        /// <summary>
        /// Checks the active items.
        /// </summary>
        /// <param name="gameItems">Game items.</param>
        /// <returns>Enumerable list of the inactive items.</returns>
        private IEnumerable<IItem> CheckForActive(List<IItem> gameItems)
        {
            return from currentItem in gameItems
               where !Helper.IsItemActive(currentItem)
               select currentItem;
        }
    }
}
