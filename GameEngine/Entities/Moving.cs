using GameEngine.Helpers;
using GameEngine.Interfaces;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains behaviour of all movable items.
    /// </summary>
    public abstract class Moving : Item, IMovable
    {
        /// <summary>
        /// Moving's vision.
        /// </summary>
        public int Vision { get; set; }

        /// <summary>
        /// Moves movable items.
        /// </summary>
        /// <param name="gameItem">Game item.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="freeCells">Free cells.</param>
        public virtual void Move(IMovable gameItem, List<IItem> gameItems, List<NewItemCoordinates> freeCells)
        {
            Walk(freeCells);
        }

        /// <summary>
        /// Relocates item by free cells.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        protected void Walk(List<NewItemCoordinates> freeCells)
        {
            NewItemCoordinates newCoordinates = WalkAround(freeCells);
            Relocate(newCoordinates.NewXCoordinate, newCoordinates.NewYCoordinate);
        }

        /// <summary>
        /// Generates random coordinates to freely move.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        /// <returns>Free coordinates to relocate item.</returns>
        private NewItemCoordinates WalkAround(List<NewItemCoordinates> freeCells)
        {
            return freeCells[Helper.random.Next(0, freeCells.Count)];
        }

        /// <summary>
        /// Relocates item by coordinates.
        /// </summary>
        /// <param name="newXCoordinate">New x coordinate.</param>
        /// <param name="newYCoordinate">New y coordinate.</param>
        protected void Relocate(int newXCoordinate, int newYCoordinate)
        {
            CoordinateX = newXCoordinate;
            CoordinateY = newYCoordinate;
        }
    }
}
