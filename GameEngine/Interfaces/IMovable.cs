using GameEngine.Entities;

namespace GameEngine.Interfaces
{
    /// <summary>
    /// The class contains behaviour of all movable items.
    /// </summary>
    public interface IMovable : IActivatable, IItem
    {
        void Move(IMovable item, List<IItem> items, List<NewItemCoordinates> freeCells);   
    }
}
