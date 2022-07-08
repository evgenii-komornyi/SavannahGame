using GameEngine.Entities.Interfaces;
using GameEngine.Services;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// Antilope entity.
    /// </summary>
    public class Antilope : Animal, IPrey
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
            Health = ConstantsRepository.AntilopeHealth;
            IsDead = false;
            Letter = ConstantsRepository.AntilopeLetter;
        }

        /// <summary>
        /// Moves current animal on the board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="animal">Animal.</param>
        /// <param name="animals">Animals.</param>
        public override void MoveAnimal(Board board, List<Animal> animals)
        {
            List<IAnimal> animalsAround = Helper.LookAround(this, animals, Vision).ToList();

            List<IPredator> predatorsAround = Helper.FindAnimalsAroundByType<IPredator>(animalsAround);

            IPredator? nearestPredator = Helper.FindNearestPredator(predatorsAround, this);
                
            List<NewAnimalCoordinates> freeCellsToMove = CalculateCorrectPosition(board, animals, this);
                
            if (nearestPredator == null)
            {
                MakeNextMove(this, freeCellsToMove);
            }
            else
            {
                if (Helper.IsPredatorNear(nearestPredator, this))
                {
                    TryToRunAway(nearestPredator, freeCellsToMove);
                }
            }
        }

        /// <summary>
        /// Calculates next move of the prey when she has found the nearest predator. 
        /// </summary>
        /// <param name="nearestPredator">Nearest predator.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        public void TryToRunAway(IPredator nearestPredator, List<NewAnimalCoordinates> freeCellsToMove)
        {
            CoordinateX = freeCellsToMove[Helper.CalculateMaxDistanceFromPredatorByFreeCells(freeCellsToMove, nearestPredator)].NewXCoordinate;
            CoordinateY = freeCellsToMove[Helper.CalculateMaxDistanceFromPredatorByFreeCells(freeCellsToMove, nearestPredator)].NewYCoordinate;
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
                child = new Antilope
                {
                    CoordinateX = birthCoordinates.NewXCoordinate,
                    CoordinateY = birthCoordinates.NewYCoordinate
                };
            }

            return child;
        }
    }
}