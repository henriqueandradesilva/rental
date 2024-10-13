namespace CrossCutting.Interfaces;

public interface IOutputPortWithNotFound<T>
{
    void NotFound();

    void Error();

    void Ok(
        T obj);
}