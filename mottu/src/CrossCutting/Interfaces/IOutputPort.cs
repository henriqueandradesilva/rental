namespace CrossCutting.Interfaces;

public interface IOutputPort<T>
{
    void Error();

    void Ok(
        T obj);
}