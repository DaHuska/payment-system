namespace is_payment_system.Services
{
    public interface IEntityAdditionService<T>
    {
        bool Add(T entity);
        bool Exists(T entity);
    }
}