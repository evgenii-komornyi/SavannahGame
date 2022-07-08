using GameEngine.Entities;

namespace GameEngine.Interfaces
{
    /// <summary>
    /// The class contains behaviour of the reproducable objects.
    /// </summary>
    public interface IReproducable
    {
        /// <summary>
        /// Reproduces new child.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="pair">Pair.</param>
        /// <returns>New game object by type.</returns>
        IItem? Reproduce(Board board, List<IItem> gameObjects, List<Pair> pair);
    }
}
