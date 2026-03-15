using System;

namespace UIComponents
{
    /// <summary>
    /// Deprecated asset loading interface. Extends <see cref="IAssetSource"/>
    /// for backward compatibility — existing resolver implementations can be
    /// used as <see cref="IAssetSource"/> providers.
    /// </summary>
    [Obsolete("Use IAssetSource instead.")]
    public interface IAssetResolver : IAssetSource
    {
    }
}
