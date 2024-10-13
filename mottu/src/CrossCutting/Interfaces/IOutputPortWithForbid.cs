namespace CrossCutting.Interfaces;

public interface IOutputPortWithForbid<T>
{
    void Forbid();

    void Error();

    void Ok(
        T obj);
}