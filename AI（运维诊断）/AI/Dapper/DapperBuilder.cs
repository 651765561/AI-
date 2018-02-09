using Microsoft.Extensions.DependencyInjection;

namespace UCFAR.Util.Dapper
{
    public class DapperBuilder : IDapperBuilder
    {
        /// <summary>
        ///  Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }

        public DapperBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
