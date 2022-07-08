using GameEngine.Entities;
using GameEngine.Helpers;
using GameEngine.Interfaces;

namespace GameEngine.Services.Managers
{
    /// <summary>
    /// The class contains behaviour of the movable game objects.
    /// </summary>
    public class MovementManager : IMovementManager
    {
        /// <summary>
        /// Moves game objects on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="gameObjects">Game objects.</param>
        public void Act(List<IItem> gameObjects, Board board)
        {
            foreach (var item in gameObjects.Cast<IMovable>())
            {
                if (Helper.IsObjectActive(item))
                {
                    List<NewObjectCoordinates> freeCellsToMove = Helper.CalculateCorrectPosition(board, gameObjects, item);

                    item.Move(item, gameObjects, freeCellsToMove);
                }
            }
        }
    }
}
