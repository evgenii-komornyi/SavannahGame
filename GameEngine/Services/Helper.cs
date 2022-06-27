using GameEngine.Entities;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains common methods.
    /// </summary>
    public class Helper
    {
        public static Random random = new Random();

        /// <summary>
        /// Checks whether cell is occupied.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>Is cell occupied.</returns>
        public static bool IsCellOccupied(int newXPosition, int newYPosition, List<Animal> animals)
        {
            return (from animal in animals
                    where animal.CoordinateX.Equals(newXPosition) &&
                          animal.CoordinateY.Equals(newYPosition)
                    select animal).Any();
        }
    }
}
