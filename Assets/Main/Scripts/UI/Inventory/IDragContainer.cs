namespace AMAZON.UI.Inventory
{
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {

    }
}