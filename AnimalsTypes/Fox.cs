using GameEngine.Entities;
using GameEngine.Helpers;
using Repository;

namespace AnimalsTypes
{
    /// <summary>
    /// Fox entity.
    /// </summary>
    public class Fox : Carnivore
    {
        /// <summary>
        /// Fox entity.
        /// </summary>
        public Fox()
        {
            Id = GenerateId();
            Sex = (AnimalSex)Helper.random.Next(2);
            IsPaired = false;
            Vision = ConstantsRepository.FoxVision;
            Health = ConstantsRepository.MaxHealth;
            IsActive = true;
            IsVisible = true;
            Letter = ConsoleKey.F;
            Color = ConsoleColor.DarkYellow;
            Specie = ConstantsRepository.Fox;
        }
    }
}