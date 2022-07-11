using GameEngine.Entities;
using GameEngine.Helpers;
using Repository;

namespace AnimalsTypes
{
    /// <summary>
    /// Lion entity.
    /// </summary>
    public class Lion : Carnivore
    {
        /// <summary>
        /// Lion entity.
        /// </summary>
        public Lion()
        {
            Id = GenerateId();
            Sex = (AnimalSex)Helper.random.Next(2);
            IsPaired = false;
            Vision = ConstantsRepository.LionVision;
            Health = ConstantsRepository.MaxHealth;
            IsActive = true;
            IsVisible = true;
            Letter = ConsoleKey.L;
            Color = ConsoleColor.DarkYellow;
            Specie = "Lion";
        }
    }
}