namespace GenericRepository.Lib.Interface;

public interface IEntity<T>
{
    T Id { get; }
}