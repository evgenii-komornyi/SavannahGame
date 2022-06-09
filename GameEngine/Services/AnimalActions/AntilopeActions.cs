using GameEngine.Entities;
using Repository;

namespace GameEngine.Services
{
    /// <summary>
    /// The class contains all logic of the antilopes' behaviour. 
    /// </summary>
    public class AntilopeActions : AnimalActions
    {
        /// <summary>
        /// Finds nearest lion to the antilope.
        /// </summary>
        /// <param name="lionsAround">Lions around.</param>
        /// <param name="antilope">Antilope.</param>
        /// <returns>Nearest lion.</returns>
        public IAnimal? FindNearestLion(List<IAnimal> lionsAround, Antilope antilope)
        {
            IAnimal? nearestLion = null;

            if (lionsAround.Count != 0)
            {
                nearestLion = CalculateMinDistanceToLion(lionsAround, antilope);
            }

            return nearestLion;
        }

        /// <summary>
        /// Calculates minimal distance to the lion.
        /// </summary>
        /// <param name="lionsAround">Lions around.</param>
        /// <param name="antilope">Antilope.</param>
        /// <returns>Nearest lion to the antilope.0</returns>
        private IAnimal? CalculateMinDistanceToLion(List<IAnimal> lionsAround, Antilope antilope)
        {
            double nearestLionDistance;
            int counter = 0;
            int nearestLionIndex = 0;
            double minLionDistance = Double.MaxValue;

            foreach (var lion in lionsAround)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates();
                pointsCoordinates.FirstXPoint = lion.CoordinateX;
                pointsCoordinates.SecondXPoint = lion.CoordinateX;
                pointsCoordinates.FirstYPoint = lion.CoordinateY;
                pointsCoordinates.SecondYPoint = lion.CoordinateY;

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
        /// Calculates next move of the antilope when she has found the nearest lion. 
        /// </summary>
        /// <param name="nearestLion">Lion.</param>
        /// <param name="antilope">Antilope.</param>
        /// <param name="freeCellsToMove">Free cells to move.</param>
        public void TryToRunAway(IAnimal nearestLion, Antilope antilope, List<NewAnimalCoordinates> freeCellsToMove)
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
        private int CalculateMaxDistanceFromLionByFreeCells(List<NewAnimalCoordinates> freeCellsToMove, IAnimal lion)
        {
            double distance;
            int counter = 0;
            int farthestFreeCellIndex = 0;
            double maxDistance = 0;

            foreach (var cell in freeCellsToMove)
            {
                PointsCoordinates pointsCoordinates = new PointsCoordinates();
                pointsCoordinates.FirstXPoint = cell.NewXCoordinate;
                pointsCoordinates.SecondXPoint = lion.CoordinateX;
                pointsCoordinates.FirstYPoint = cell.NewYCoordinate;
                pointsCoordinates.SecondYPoint = lion.CoordinateY;

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
    }
}
