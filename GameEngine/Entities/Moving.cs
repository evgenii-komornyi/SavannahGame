using GameEngine.Helpers;
using GameEngine.Interfaces;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains behaviour of all movable objects.
    /// </summary>
    public abstract class Moving : Item, IMovable
    {
        /// <summary>
        /// Moving's vision.
        /// </summary>
        public int Vision { get; set; }

        /// <summary>
        /// Moves movable objects.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="freeCells">Free cells.</param>
        public virtual void Move(IMovable gameObject, List<IItem> gameObjects, List<NewObjectCoordinates> freeCells)
        {
            Walk(freeCells);
        }

        /// <summary>
        /// Relocates object by free cells.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        protected void Walk(List<NewObjectCoordinates> freeCells)
        {
            NewObjectCoordinates newCoordinates = WalkAround(freeCells);
            Relocate(newCoordinates.NewXCoordinate, newCoordinates.NewYCoordinate);
        }

        /// <summary>
        /// Generates random coordinates to freely move.
        /// </summary>
        /// <param name="freeCells">Free cells.</param>
        /// <returns>Free coordinates to relocate object.</returns>
        private NewObjectCoordinates WalkAround(List<NewObjectCoordinates> freeCells)
        {
            return freeCells[Helper.random.Next(0, freeCells.Count)];
        }

        /// <summary>
        /// Relocates object by coordinates.
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
