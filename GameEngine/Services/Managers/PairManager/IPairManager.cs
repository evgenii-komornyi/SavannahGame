using GameEngine.Entities;
using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains logic to manage animals' pairs.
    /// </summary>
    public interface IPairManager
    {
        /// <summary>
        /// Creates new pair for current pairable item, searching opposite free pairable item near by this item. 
        /// </summary>
        /// <param name="pairablesItems">Pairable items.</param>
        /// <returns>New pair.</returns>
        Pair CreatePair(List<IItem> pairablesItems);

        /// <summary>
        /// Added pair to list.
        /// </summary>
        /// <param name="newPair">New pair.</param>
        /// <param name="pairs">Pairs.</param>
        void AddPairToList(Pair newPair, List<Pair> pairs);

        /// <summary>
        /// Checks pair for existance, and if it is true, then item can reproduce new item after 3 consecutive rounds. 
        /// </summary>
        /// <param name="pairsToDestroy">Pairs to destroy.</param>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="reproducedItems">Reproduced items.</param>
        void CheckPairForExistence(List<Pair> pairsToDestroy, Board board, List<IItem> gameItems, List<IItem> reproducedItems);

        /// <summary>
        /// Removes pairs that end their existence. 
        /// </summary>
        void RemoveNotExistingPairs(List<Pair> pairs);
    }
}
