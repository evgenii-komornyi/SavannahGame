using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// Lion entity.
    /// </summary>
    public class Lion : IAnimal
    {
        public Lion()
        {
            Id = GenerateId();
            Type = AnimalType.Lion;
            Vision = ConstantsRepository.LionVision;
            IsDead = false;
            Letter = ConstantsRepository.LionLetter;
        }

        public int Id { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public AnimalType Type { get; set; }
        public int Vision { get; set; }
        public bool IsDead { get; set; }
        public string Letter { get; set; }

        private static int id = 1;
        private int GenerateId()
        {
            return id++;
        }

    }
}