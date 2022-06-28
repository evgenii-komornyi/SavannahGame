using GameEngine.Services;
using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// Lion entity.
    /// </summary>
    public class Lion : Animal
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
        /// <param name="animal">Animal.</param>
        /// <param name="animals">Animals.</param>
        public override void MoveAnimal(Board board, Animal animal, List<Animal> animals)
        {
            Lion lion = (Lion)animal;

            List<Antilope> antilopesAround = FindAnimalsAroundByType<Antilope>(lion, animals);

            Animal? antilopeToHunt = FindNearestAntilope(antilopesAround, lion);

            List<NewAnimalCoordinates> freeCellsToMove = CalculateCorrectPosition(board, animals, lion);

            if (antilopeToHunt == null)
            {
                MakeNextMove(lion, freeCellsToMove);
            }
            else
            {
                if (IsAntilopeFar(lion, antilopeToHunt))
                {
                    HuntAntilope(antilopeToHunt, lion, freeCellsToMove);
                }
                else
                {
                    EatAntilope(antilopeToHunt, lion);
                }
            }
        }

        /// <summary>
        /// Finds the nearest antilope to lion's position.
        /// </summary>
        /// <param name="antilopesAround">Antilopes around.</param>
        /// <param name="lion">Lion.</param>
        /// <returns>Nearest antilope to hunt.</returns>
        private Animal? FindNearestAntilope(List<Antilope> antilopesAround, Lion lion)
        {
            Animal? nearestAntilope = null;

            if (antilopesAround.Count != 0)
            {
                nearestAntilope = CalculateMinDistanceToAntilope(antilopesAround, lion);
            }

            return nearestAntilope;
        }

        /// <summary>
        /// Calculates minimal distance to the antilope among antilopes in the lion's vision by Pthagoryan theorem. 
        /// </summary>
        /// <param name="antilopesAround">Antilopes are around.</param>
        /// <param name="lion">Lion.</param>
        /// <returns>Nearest antilope to lion.</returns>
        private Animal? CalculateMinDistanceToAntilope(List<Antilope> antilopesAround, Animal lion)
        {
            double distance;
            int counter = 0;
            int nearestAntilopeIndex = 0;
            double minAntilopeDistance = Double.MaxValue;

            foreach (var antilope in antilopesAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = lion.CoordinateX,
                    SecondXPoint = antilope.CoordinateX,
                    FirstYPoint = lion.CoordinateY,
                    SecondYPoint = antilope.CoordinateY
                };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minAntilopeDistance > distance)
                {
                    minAntilopeDistance = distance;
                    nearestAntilopeIndex = counter;
                }
                counter++;
            }

            return antilopesAround[nearestAntilopeIndex];
        }

        /// <summary>
        /// Checks the antilope far the lion.
        /// </summary>
        /// <param name="lion">Lion.</param>
        /// <param name="antilopeToHunt">Far antilope to hunt.</param>
        /// <returns>Is antilope far.</returns>
        private bool IsAntilopeFar(Lion lion, Animal antilopeToHunt)
        {
            return Math.Abs(lion.CoordinateX - antilopeToHunt.CoordinateX) > 1 ||
                   Math.Abs(lion.CoordinateY - antilopeToHunt.CoordinateY) > 1;
        }

        /// <summary>
        /// Calculates next move of the lion when he has found the antilope to hunt. 
        /// </summary>
        /// <param name="nearestAntilope">Antilope.</param>
        /// <param name="lion">Lion.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        private void HuntAntilope(Animal nearestAntilope, Lion lion, List<NewAnimalCoordinates> freeCellsToMove)
        {
            lion.CoordinateX = freeCellsToMove[CalculateMinDistanceToAntilopeByFreeCells(freeCellsToMove, nearestAntilope)].NewXCoordinate;
            lion.CoordinateY = freeCellsToMove[CalculateMinDistanceToAntilopeByFreeCells(freeCellsToMove, nearestAntilope)].NewYCoordinate;
        }

        /// <summary>
        /// Calculates minimal distance to the antilope by free cells.
        /// </summary>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        /// <param name="antilope">Antilope.</param>
        /// <returns>Index of minimal distance cell.</returns>
        private int CalculateMinDistanceToAntilopeByFreeCells(List<NewAnimalCoordinates> freeCellsToMove, Animal antilope)
        {
            double distance;
            int counter = 0;
            int nearestFreeCellIndex = 0;
            double minDistance = Double.MaxValue;

            foreach (var cell in freeCellsToMove)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates
                {
                    FirstXPoint = cell.NewXCoordinate,
                    SecondXPoint = antilope.CoordinateX,
                    FirstYPoint = cell.NewYCoordinate,
                    SecondYPoint = antilope.CoordinateY
            };

                distance = CalculateSquareDistanceByPythagoras(pointsCoordinates);

                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestFreeCellIndex = counter;
                }
                counter++;
            }

            return nearestFreeCellIndex;
        }

        /// <summary>
        /// Moves the lion to the antilope's position and eat it.
        /// </summary>
        /// <param name="antilope">Antilope.</param>
        /// <param name="lion">Lion.</param>
        private void EatAntilope(Animal antilope, Lion lion)
        {
            antilope.Health = antilope.Health <= 0 ? 0 : antilope.Health - 10;
            lion.Health = lion.Health >= ConstantsRepository.LionHealth ? ConstantsRepository.LionHealth : lion.Health + 10;

            if (antilope.Health == 0)
            {
                lion.CoordinateX = antilope.CoordinateX;
                lion.CoordinateY = antilope.CoordinateY;
                antilope.IsDead = true;
                lion.Health = ConstantsRepository.LionHealth;
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
            NewAnimalCoordinates birthCoordinates = CalculateFreeCellsToBirth(board, animals);
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