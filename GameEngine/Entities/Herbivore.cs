using GameEngine.Helpers;
using GameEngine.Interfaces;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// The class contains behaviour of the Herbivores. 
    /// </summary>
    public abstract class Herbivore : Animal
    {
        /// <summary>
        /// Moves movable item. 
        /// </summary>
        /// <param name="movableItem">Movable item.</param>
        /// <param name="gameItems">Game items.</param>
        /// <param name="freeCells">Free cells.</param>
        public override void Move(IMovable movableItem, List<IItem> gameItems, List<NewItemCoordinates> freeCells)
        {
            List<Animal> animalsAround = Helper.LookAround(movableItem, gameItems, movableItem.Vision).Cast<Animal>().ToList();
            List<Carnivore> carnivoresAround = Helper.FindItemsAroundByType<Carnivore>(animalsAround);

            Carnivore? carnivore = Helper.FindNearestCarnivore(carnivoresAround, this);

            GetDamage(ConstantsRepository.HealthDecreaser);

            if (carnivore == null)
            {
                Walk(freeCells);
            }
            else
            {
                if (Helper.IsHunterNear(carnivore, this))
                {
                    RunAway(carnivore, freeCells);
                }
            }
        }

        /// <summary>
        /// Runs away herbivore from nearest hunter.
        /// </summary>
        /// <param name="nearestHunter">Nearest hunter.</param>
        /// <param name="freeCells">Free cells.</param>
        protected virtual void RunAway(Carnivore nearestHunter, List<NewItemCoordinates> freeCells)
        {
            Relocate(freeCells[Helper.CalculateMaxDistanceFromCarnivoreByFreeCells(freeCells, nearestHunter)].NewXCoordinate,
                freeCells[Helper.CalculateMaxDistanceFromCarnivoreByFreeCells(freeCells, nearestHunter)].NewYCoordinate);
        }
    }
}
