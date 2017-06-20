namespace Symlconnect.DataModel
{
    /// <summary>
    ///     Resolves a value from a given IEntity, base value and ISessionContext.
    /// </summary>
    /// <remarks>
    ///     Individual Value Providers can add additional properties to determine how the value should be resolved. Value
    ///     Providers may also support nesting in order to combine with other value providers (e.g. if conditionals,
    ///     comparisons, calculations)
    /// </remarks>
    public interface IValueProvider
    {
        ValueKind ValueProviderKind { get; set; }
        object ResolveValue(IEntity entity, object baseValue, ISessionContext sessionContext);
    }
}