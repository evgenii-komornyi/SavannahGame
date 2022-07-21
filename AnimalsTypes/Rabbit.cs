using GameEngine.Entities;
using GameEngine.Helpers;
using Repository;

namespace AnimalsTypes
{
    /// <summary>
    /// Rabbit entity.
    /// </summary>
    public class Rabbit : Herbivore
    {
        /// <summary>
        /// Rabbit entity.
        /// </summary>
        public Rabbit()
        {
            Id = GenerateId();
            Sex = (AnimalSex)Helper.random.Next(2);
            IsPaired = false;
            Vision = ConstantsRepository.RabbitVision;
            Health = ConstantsRepository.MaxHealth;
            IsActive = true;
            IsVisible = true;
            Letter = ConsoleKey.R;
            Color = ConsoleColor.White;
            Specie = ConstantsRepository.Rabbit;
        }
    }
}