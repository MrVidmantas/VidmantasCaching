namespace Vidmantas.Caching.Helpers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion

    public static class EnumerableExtensions
    {
        #region Public Methods

        public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> func)
        {
            foreach (var value in source)
            {
                await func(value);
            }
        }

        #endregion
    }
}
