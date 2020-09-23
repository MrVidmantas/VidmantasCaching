namespace Vidmantas.Caching.Interfaces
{
    public interface ICacheKey
    {
        #region Properties

        string ParentKey { get; set; }

        string ValueKey { get; set; }

        #endregion
    }
}
