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
        /// Creates new pair for current pairable object, searching opposite free pairable object near by this object. 
        /// </summary>
        /// <param name="pairables">Pairables.</param>
        /// <returns>New pair.</returns>
        Pair CreatePair(List<IItem> pairables);

        /// <summary>
        /// Checks pair for existance, and if it is true, then object can reproduce new object after 3 consecutive rounds. 
        /// </summary>
        /// <param name="pairsToDestroy">Pairs to destroy.</param>
        /// <param name="board">Board.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="reproducedObjects">Reproduced objects.</param>
        void CheckPairForExistence(List<Pair> pairsToDestroy, Board board, List<IItem> gameObjects, List<IItem> reproducedObjects);

        /// <summary>
        /// Removes pairs that end their existence. 
        /// </summary>
        void RemoveNotExistingPairs(List<Pair> pairs);
    }
}
