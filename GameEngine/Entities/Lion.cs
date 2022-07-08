using GameEngine.Entities.Interfaces;
using GameEngine.Services;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// Lion entity.
    /// </summary>
    public class Lion : Animal, IPredator
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
            Health = ConstantsRepository.LionHealth;
            IsDead = false;
            Letter = ConstantsRepository.LionLetter;
        }

        /// <summary>
        /// Moves current animal on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        public override void MoveAnimal(Board board, List<Animal> animals)
        {
            List<IAnimal> animalsAround = Helper.LookAround(this, animals, Vision).ToList();

            List<IPrey> preysAround = Helper.FindAnimalsAroundByType<IPrey>(animalsAround);

            IPrey? preyToHunt = Helper.FindNearestPrey(preysAround, this);

            List<NewAnimalCoordinates> freeCellsToMove = CalculateCorrectPosition(board, animals, this);

            if (preyToHunt == null)
            {
                MakeNextMove(this, freeCellsToMove);
            }
            else
            {
                if (Helper.IsPreyFar(preyToHunt, this))
                {
                    HuntPrey(preyToHunt, freeCellsToMove);
                }
                else
                {
                    EatPrey(preyToHunt);
                }
            }
        }

        /// <summary>
        /// Calculates next move of the predator when he has found the prey to hunt. 
        /// </summary>
        /// <param name="nearestPrey">Prey.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        public void HuntPrey(IPrey nearestPrey, List<NewAnimalCoordinates> freeCellsToMove)
        {
            CoordinateX = freeCellsToMove[Helper.CalculateMinDistanceToPreyByFreeCells(freeCellsToMove, nearestPrey)].NewXCoordinate;
            CoordinateY = freeCellsToMove[Helper.CalculateMinDistanceToPreyByFreeCells(freeCellsToMove, nearestPrey)].NewYCoordinate;
        }

        /// <summary>
        /// Moves the predator to the prey's position and eat it.
        /// </summary>
        /// <param name="prey">Prey.</param>
        public void EatPrey(IPrey prey)
        {
            prey.Health = prey.Health <= 0 ? 0 : prey.Health - 10;
            Health = Health >= ConstantsRepository.LionHealth ? ConstantsRepository.LionHealth : Health + 10;

            if (prey.Health == 0)
            {
                CoordinateX = prey.CoordinateX;
                CoordinateY = prey.CoordinateY;
                prey.IsDead = true;
                Health = ConstantsRepository.LionHealth;
            }
        }

        /// <summary>
        /// Gives birth of a new animal.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animals">Animals.</param>
        /// <returns>New animal (child).</returns>
        public override Animal? GiveBirth(Board board, List<Animal> animals)
        {
            NewAnimalCoordinates? birthCoordinates = CalculateFreeCellsToBirth(board, animals);
            Animal? child = null;

            if (birthCoordinates != null)
            {
                child = new Lion
                {
                    CoordinateX = birthCoordinates.NewXCoordinate,
                    CoordinateY = birthCoordinates.NewYCoordinate
                };
            }
            
            return child;
        }
    }
}