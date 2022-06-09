namespace GameEngine.Entities
{
    /// <summary>
    /// Animal entity.
    /// </summary>
    public interface IAnimal
    {
        int Id { get; set; }
        int CoordinateX { get; set; }
        int CoordinateY { get; set; }
        AnimalType Type { get; set; }
        int Vision { get; set; }
        bool IsDead { get; set; }
        string Letter { get; set; }
    }
}