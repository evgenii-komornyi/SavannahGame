using GameEngine.Entities;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains common methods.
    /// </summary>
    public class Helper
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates a random number in the interval.
        /// </summary>
        /// <param name="minLimit">Min limit.</param>
        /// <param name="maxLimit">Max limit.</param>
        /// <returns>Random number.</returns>
        public static int GenerateRandomCoordinates(int minLimit, int maxLimit)
        {
            return _random.Next(minLimit, maxLimit);
        }

        /// <summary>
        /// Checks non-free place for new animal's step.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>The first non-free place for animal.</returns>
        public static bool IsNotFreePlace(int newXPosition, int newYPosition, List<Animal> animals)
        {
            return (from animal in animals
                    where animal.CoordinateX.Equals(newXPosition) &&
                          animal.CoordinateY.Equals(newYPosition)
                    select animal).Any();
        }
    }
}
