using GameEngine.Helpers;
using GameEngine.Interfaces;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains behaviour of the Carnivores.
    /// </summary>
    public abstract class Carnivore : Animal, IHunter
    {
        /// <summary>
        /// Moves movable item.
        /// </summary>
        /// <param name="movableItem">Movable item.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="freeCells">Free cells.</param>
        public override void Move(IMovable movableItem, List<IItem> gameItems, List<NewItemCoordinates> freeCells)
        {
            List<Animal> itemsAround = Helper.LookAround(movableItem, gameItems, movableItem.Vision).Cast<Animal>().ToList();
            List<Herbivore> herbivoresAround = Helper.FindItemsAroundByType<Herbivore>(itemsAround);

            Herbivore? herbivore = Helper.FindNearestItem(herbivoresAround, this);

            GetDamage(ConstantsRepository.HealthDecreaser);
            
            if (herbivore == null)
            {
                Walk(freeCells);
            } 
            else
            {
                if (Helper.IsFoodFar(herbivore, this))
                {
                    Hunt(herbivore, freeCells);
                }
                else
                {
                    Eat(herbivore);
                }
            }
        }

        /// <summary>
        /// Relocates carnivore to the nearest cell of animal. 
        /// </summary>
        /// <param name="nearestAnimal">Nearest animal.</param>
        /// <param name="freeCells">Free cells.</param>
        public void Hunt(Animal nearestAnimal, List<NewItemCoordinates> freeCells)
        {
            Relocate(freeCells[Helper.CalculateMinDistanceToFoodByFreeCells(freeCells, nearestAnimal)].NewXCoordinate,
                freeCells[Helper.CalculateMinDistanceToFoodByFreeCells(freeCells, nearestAnimal)].NewYCoordinate);
        }

        /// <summary>
        /// Relocates carnivore to the cell of the animal, that was killed.
        /// </summary>
        /// <param name="animalToKill">Animal to kill.</param>
        public virtual void Kill(Animal animalToKill)
        {
            Relocate(animalToKill.CoordinateX, animalToKill.CoordinateY);
        }

        /// <summary>
        /// Decreases health of prey and increases self heath. 
        /// </summary>
        /// <param name="animalToEat">Animal to eat.</param>
        private void Eat(Animal animalToEat)
        {
            animalToEat.Health = animalToEat.Health <= 0 ? 0 : Attack(animalToEat, 10);
            Health = Health >= ConstantsRepository.MaxHealth ? ConstantsRepository.MaxHealth : Heal(10);

            if (!animalToEat.IsActive)
            {
                Kill(animalToEat);
                Heal();
            }
        }
    }
}
