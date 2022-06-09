using GameEngine.Entities;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains all logic of the lions' behaviour.
    /// </summary>
    public class LionActions : AnimalActions
    {
        /// <summary>
        /// Finds the nearest antilope to lion's position.
        /// </summary>
        /// <param name="animals">Animals.</param>
        /// <param name="lion">Lion.</param>
        /// <returns>Nearest antilope to hunt.</returns>
        public IAnimal? FindNearestAntilope(List<IAnimal> antilopesAround, Lion lion)
        {
            IAnimal? nearestAntilope = null;

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
        private IAnimal? CalculateMinDistanceToAntilope(List<IAnimal> antilopesAround, IAnimal lion)
        {
            double distance;
            int counter = 0;
            int nearestAntilopeIndex = 0;
            double minAntilopeDistance = Double.MaxValue;
            
            foreach (var antilope in antilopesAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates();
                pointsCoordinates.FirstXPoint = lion.CoordinateX;
                pointsCoordinates.SecondXPoint = antilope.CoordinateX;
                pointsCoordinates.FirstYPoint = lion.CoordinateY;
                pointsCoordinates.SecondYPoint = antilope.CoordinateY;

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
        /// Calculates next move of the lion when he has found the antilope to hunt. 
        /// </summary>
        /// <param name="nearestAntilope">Antilope.</param>
        /// <param name="lion">Lion.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        public void HuntAntilope(IAnimal nearestAntilope, Lion lion, List<NewAnimalCoordinates> freeCellsToMove)
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
        private int CalculateMinDistanceToAntilopeByFreeCells(List<NewAnimalCoordinates> freeCellsToMove, IAnimal antilope)
        {
            double distance;
            int counter = 0;
            int nearestFreeCellIndex = 0;
            double minDistance = Double.MaxValue;

            foreach (var cell in freeCellsToMove)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates();
                pointsCoordinates.FirstXPoint = cell.NewXCoordinate;
                pointsCoordinates.SecondXPoint = antilope.CoordinateX;
                pointsCoordinates.FirstYPoint = cell.NewYCoordinate;
                pointsCoordinates.SecondYPoint = antilope.CoordinateY;

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
        public void EatAntilope(IAnimal antilope, Lion lion)
        {
            lion.CoordinateX = antilope.CoordinateX;
            lion.CoordinateY = antilope.CoordinateY;
            antilope.IsDead = true;
        }
    }
}