using GameEngine.Entities;
using GameEngine.Helpers;
using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains behaviour of the movable game items.
    /// </summary>
    public class MovementManager : IMovementManager
    {
        /// <summary>
        /// Moves game items on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameItems">Game items.</param>
        public void Act(List<IItem> gameItems, Board board)
        {
            foreach (var item in gameItems.Cast<IMovable>())
            {
                if (Helper.IsItemActive(item))
                {
                    List<NewItemCoordinates> freeCellsToMove = Helper.CalculateCorrectPosition(board, gameItems, item);

                    item.Move(item, gameItems, freeCellsToMove);
                }
            }
        }
    }
}
