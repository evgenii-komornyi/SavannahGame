using Repository;

namespace GameEngine.Entities
{
    /// <summary>
    /// Antilope entity.
    /// </summary>
    public class Antilope : IAnimal
    {
        public Antilope()
        {
            Id = GenerateId();
            Type = AnimalType.Antilope;
            Vision = ConstantsRepository.AntilopeVision;
            IsDead = false;
            Letter = ConstantsRepository.AntilopeLetter;
        }

        public int Id { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public AnimalType Type { get; set; }
        public int Vision { get; set; }
        public bool IsDead { get; set; }
        public string Letter { get; set; }

        private static int id = 0;
        private int GenerateId()
        {
            return id++;
        }
    }
}