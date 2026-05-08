namespace StudyDotnet.Domain.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
