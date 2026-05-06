namespace StudyDotnet.Api.Domain;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
