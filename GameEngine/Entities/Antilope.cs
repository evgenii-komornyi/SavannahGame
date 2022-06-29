using GameEngine.Services;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// Antilope entity.
    /// </summary>
    public class Antilope : Animal
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
        public override void MoveAnimal(Board board, Animal animal, List<Animal> animals)
        {
            Antilope antilope = (Antilope)animal;

            List<Lion> lionsAround = FindAnimalsAroundByType<Lion>(antilope, animals);

            Animal? nearestLion = FindNearestLion(lionsAround);
                
            List<NewAnimalCoordinates> freeCellsToMove = CalculateCorrectPosition(board, animals, antilope);
                
            if (nearestLion == null)
            {
                MakeNextMove(antilope, freeCellsToMove);
            }
            else
            {
                if (IsLionNear(antilope, nearestLion))
                {
                    TryToRunAway(nearestLion, antilope, freeCellsToMove);
                }
            }
        }

        /// <summary>
        /// Finds nearest lion to the antilope.
        /// </summary>
        /// <param name="lionsAround">Lions around.</param>
        /// <returns>Nearest lion.</returns>
        private Animal? FindNearestLion(List<Lion> lionsAround)
        {
            Animal? nearestLion = null;

            if (lionsAround.Count != 0)
            {
                nearestLion = CalculateMinDistanceToLion(lionsAround);
            }

            return nearestLion;
        }

        /// <summary>
        /// Calculates minimal distance to the lion.
        /// </summary>
        /// <param name="lionsAround">Lions around.</param>
        /// <returns>Nearest lion to the antilope.0</returns>
        private Animal? CalculateMinDistanceToLion(List<Lion> lionsAround)
        {
            double nearestLionDistance;
            int counter = 0;
            int nearestLionIndex = 0;
            double minLionDistance = Double.MaxValue;

            foreach (var lion in lionsAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = lion.CoordinateX,
                    SecondXPoint = lion.CoordinateX,
                    FirstYPoint = lion.CoordinateY,
                    SecondYPoint = lion.CoordinateY
                };

                nearestLionDistance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minLionDistance > nearestLionDistance)
                {
                    minLionDistance = nearestLionDistance;
                    nearestLionIndex = counter;
                }
                counter++;
            }

            return lionsAround[nearestLionIndex];
        }

        /// <summary>
        /// Checks the nearest lion to antilope.
        /// </summary>
        /// <param name="antilope">Antilope.</param>
        /// <param name="nearestLion">Nearest lion.</param>
        /// <returns>Is lion near.</returns>
        private bool IsLionNear(Antilope antilope, Animal nearestLion)
        {
            return Math.Abs(antilope.CoordinateX - nearestLion.CoordinateX) < 2 ||
                   Math.Abs(antilope.CoordinateY - nearestLion.CoordinateY) < 2;
        }

        /// <summary>
        /// Calculates next move of the antilope when she has found the nearest lion. 
        /// </summary>
        /// <param name="nearestLion">Lion.</param>
        /// <param name="antilope">Antilope.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        private void TryToRunAway(Animal nearestLion, Antilope antilope, List<NewAnimalCoordinates> freeCellsToMove)
        {
            antilope.CoordinateX = freeCellsToMove[CalculateMaxDistanceFromLionByFreeCells(freeCellsToMove, nearestLion)].NewXCoordinate;
            antilope.CoordinateY = freeCellsToMove[CalculateMaxDistanceFromLionByFreeCells(freeCellsToMove, nearestLion)].NewYCoordinate;
        }

        /// <summary>
        /// Calculates maximal distance to the lion by free cells.
        /// </summary>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        /// <param name="lion">Lion.</param>
        /// <returns>Index of maximal distance cell.</returns>
        private int CalculateMaxDistanceFromLionByFreeCells(List<NewAnimalCoordinates> freeCellsToMove, Animal lion)
        {
            double distance;
            int counter = 0;
            int farthestFreeCellIndex = 0;
            double maxDistance = 0;

            foreach (var cell in freeCellsToMove)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = cell.NewXCoordinate,
                    SecondXPoint = lion.CoordinateX,
                    FirstYPoint = cell.NewYCoordinate,
                    SecondYPoint = lion.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (maxDistance < distance)
                {
                    maxDistance = distance;
                    farthestFreeCellIndex = counter;
                }
                counter++;
            }

            return farthestFreeCellIndex;
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