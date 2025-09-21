namespace AMAZON.UI.Inventories
{
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {

    }
}