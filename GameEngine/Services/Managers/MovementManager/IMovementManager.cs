using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains behaviour of the movable game objects.
    /// </summary>
    public interface IMovementManager
    {
        /// <summary>
        /// Moves game objects on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameObjects">Game objects.</param>
        void Act(List<IItem> gameObjects, Board board);
    }
}
