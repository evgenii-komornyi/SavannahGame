using GameEngine.Entities;
using GameEngine.Helpers;
using Repository;

namespace AnimalsTypes
{
    /// <summary>
    /// Antilope entity.
    /// </summary>
    public class Antilope : Herbivore
    {       
        /// <summary>
        /// Antilope entity.
        /// </summary>
        public Antilope()
        {
            Id = GenerateId();
            Sex = (AnimalSex)Helper.random.Next(2);
            IsPaired = false;
            Vision = ConstantsRepository.AntilopeVision;
            Health = ConstantsRepository.MaxHealth;
            IsActive = true;
            IsVisible = true;
            Letter = ConsoleKey.A;
            Color = ConsoleColor.Yellow;
            Specie = "Antilope";
        }
    }
}