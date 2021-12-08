namespace web_lab2.Abstractions
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}