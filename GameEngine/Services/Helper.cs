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
            return NearByAnimals(newXPosition, newYPosition, animals).Any();
        }

        /// <summary>
        /// Looks around board from animal based on it vision.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="allAnimals">All animals.</param>
        /// <param name="vision">How many cells animal can sense other animals.</param>
        /// <returns>Enumerable list with nearby animals.</returns>
        public static IEnumerable<Animal> LookAround(Animal currentAnimal, IEnumerable<Animal> allAnimals, int vision = 1)
        {
            return from animalForSearch in allAnimals
                   where AnimalsAreInRange(currentAnimal, animalForSearch, vision)
                   select animalForSearch;
        }

        /// <summary>
        /// Returns all animals in the range of vision, excluding itself.
        /// </summary>
        /// <param name="currentAnimal">Current animal.</param>
        /// <param name="animalForSearch">Animal for search.</param>
        /// <param name="vision">How many cells animal can sense other animals.</param>
        /// <returns>Are the animals in the range of vision, excluding itself.</returns>
        private static bool AnimalsAreInRange(Animal currentAnimal, Animal animalForSearch, int vision)
        {
            bool isOwnPosition = currentAnimal.CoordinateX == animalForSearch.CoordinateX &&
                                 currentAnimal.CoordinateY == animalForSearch.CoordinateY;
            return !isOwnPosition &&
                    animalForSearch.CoordinateX >= currentAnimal.CoordinateX - vision &&
                    animalForSearch.CoordinateX <= currentAnimal.CoordinateX + vision &&
                    animalForSearch.CoordinateY >= currentAnimal.CoordinateY - vision &&
                    animalForSearch.CoordinateY <= currentAnimal.CoordinateY + vision;
        }

        /// <summary>
        /// Checks nearby animals around animal.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>Enumerable list of found animals around.</returns>
        private static IEnumerable<Animal> NearByAnimals(int newXPosition, int newYPosition, List<Animal> animals)
        {
            return from animal in animals
                   where CheckForNearByAnimals(newXPosition, newYPosition, animal)
                   select animal;
        }

        /// <summary>
        /// Checks for nearby animals.
        /// </summary>
        /// <param name="newXPosition">New X position.</param>
        /// <param name="newYPosition">New Y position.</param>
        /// <param name="animal">Animal.</param>
        /// <returns>Is animal found another animal around it.</returns>
        private static bool CheckForNearByAnimals(int newXPosition, int newYPosition, Animal animal)
        {
            return animal.CoordinateX.Equals(newXPosition) &&
                         animal.CoordinateY.Equals(newYPosition);
        }
    }
}
