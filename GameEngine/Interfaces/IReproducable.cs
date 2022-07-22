using GameEngine.Entities;

namespace GameEngine.Interfaces
{
    /// <summary>
    /// The class contains behaviour of the reproducable items.
    /// </summary>
    public interface IReproducable
    {
        /// <summary>
        /// Reproduces new child.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="pair">Pair.</param>
        /// <returns>New game item by type.</returns>
        IItem? Reproduce(Board board, List<IItem> gameItems, List<Pair> pair);
    }
}
