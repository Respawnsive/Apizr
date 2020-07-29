namespace Apizr.Mapping
{
    /// <summary>
    /// [AutoMapper integration required] Tells Apizr that Api entity is mapped with a model entity
    /// </summary>
    /// <typeparam name="TModelEntity">The friendly model entity to play with</typeparam>
    /// <typeparam name="TApiEntity">The real api entity used for requesting</typeparam>
    public class MappedEntity<TModelEntity, TApiEntity> where TModelEntity : class where TApiEntity : class
    {
    }
}
