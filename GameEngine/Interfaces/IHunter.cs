using GameEngine.Entities;

namespace GameEngine.Interfaces
{
    /// <summary>
    /// Hunters' behaviour.
    /// </summary>
    public interface IHunter : IItem
    {
        /// <summary>
        /// Relocates hunter to the nearest cell of animal. 
        /// </summary>
        /// <param name="nearestAnimal">Nearest animal.</param>
        /// <param name="freeCells">Free cells.</param>
        void Hunt(Animal nearestAnimal, List<NewObjectCoordinates> freeCells);

        /// <summary>
        /// Relocates hunter to the cell of the animal, that was killed.
        /// </summary>
        /// <param name="animalToKill">Animal to kill.</param>
        void Kill(Animal animalToKill);
    }
}
