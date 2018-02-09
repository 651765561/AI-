using Microsoft.Extensions.Options;
using UCFAR.Util.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering core services
    /// </summary>
    public static class DapperBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the required platform services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IDapperBuilder AddRequiredPlatformServices(this IDapperBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<DapperOptions>>().Value);

            return builder;
        }
    }
}
