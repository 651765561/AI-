using System;
using UCFAR.Util.Dapper;
using  Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setUpAction">The setup action.</param>
        /// <returns></returns>
        public static IDapperBuilder AddDapper(this IServiceCollection services, Action<DapperOptions> setUpAction)
        {
            var builder = new DapperBuilder(services);

            builder.Services.Configure(setUpAction);

            builder.Services.TryAddSingleton<IDapperProvider, DapperProvider>();

            //add platform require servcies
            builder.AddRequiredPlatformServices();

            return builder;
        }
    }
}
