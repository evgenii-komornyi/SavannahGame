using GameEngine.Interfaces;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains common behaviour and properties for the animals. 
    /// </summary>
    public abstract class Animal : Moving, IPairable
    {
        /// <summary>
        /// Animal's health.
        /// </summary>
        public double Health { get; set; }

        /// <summary>
        /// Animal's sex.
        /// </summary>
        public AnimalSex Sex { get; set; }

        /// <summary>
        /// Indicates if animal is in pair.
        /// </summary>
        public bool IsPaired { get; set; }

        /// <summary>
        /// Heals the animal for maximum health.
        /// </summary>
        protected virtual void Heal() => Health = ConstantsRepository.MaxHealth;

        /// <summary>
        /// Heals the animal for the passed value.
        /// </summary>
        /// <param name="healthToIncrease">Health to increase.</param>
        /// <returns>Health after increasing.</returns>
        protected virtual double Heal(double healthToIncrease) => Health += healthToIncrease;

        /// <summary>
        /// Decreases health by damage.
        /// </summary>
        /// <param name="damage">Damage.</param>
        /// <returns>Health after decreasing.</returns>
        protected virtual double GetDamage(double damage) 
        { 
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }

            return Health;
        }

        /// <summary>
        /// Attacks the animal with damage.
        /// </summary>
        /// <param name="animalToAttack">Animal to attack.</param>
        /// <param name="damage">Damage.</param>
        /// <returns>Health after attack.</returns>
        protected virtual double Attack(Animal animalToAttack, double damage) => animalToAttack.GetDamage(damage);
    }
}
