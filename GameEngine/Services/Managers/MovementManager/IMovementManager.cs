using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains behaviour of the movable game items.
    /// </summary>
    public interface IMovementManager
    {
        /// <summary>
        /// Moves game items on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        void Act(List<IItem> gameItems, Board board);
    }
}
