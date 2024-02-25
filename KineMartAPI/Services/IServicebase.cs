namespace KineMartAPI.Services
{
    public interface IServicebase<T>
    {
        Task Add(T obj);
        Task GetAll();
        Task Update(int id,T obj);
        Task GetOne(int id);
    }
}
