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
        /// Moves movable object. 
        /// </summary>
        /// <param name="movableObject">Movable object.</param>
        /// <param name="gameObjects">Game objects.</param>
        /// <param name="freeCells">Free cells.</param>
        public override void Move(IMovable movableObject, List<IItem> gameObjects, List<NewObjectCoordinates> freeCells)
        {
            List<Animal> animalsAround = Helper.LookAround(movableObject, gameObjects, movableObject.Vision).Cast<Animal>().ToList();
            List<Carnivore> carnivoresAround = Helper.FindObjectsAroundByType<Carnivore>(animalsAround);

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
                    RunAway(carnivore, this, freeCells);
                }
            }
        }

        /// <summary>
        /// Runs away herbivore from nearest hunter.
        /// </summary>
        /// <param name="nearestHunter">Nearest hunter.</param>
        /// <param name="herbivore">Herbivore.</param>
        /// <param name="freeCells">Free cells.</param>
        protected virtual void RunAway(Carnivore nearestHunter, Herbivore herbivore, List<NewObjectCoordinates> freeCells)
        {
            Relocate(freeCells[Helper.CalculateMaxDistanceFromCarnivoreByFreeCells(freeCells, nearestHunter)].NewXCoordinate,
                freeCells[Helper.CalculateMaxDistanceFromCarnivoreByFreeCells(freeCells, nearestHunter)].NewYCoordinate);
        }
    }
}
