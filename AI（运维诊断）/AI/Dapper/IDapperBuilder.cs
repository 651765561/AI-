using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IDapperBuilder
    {
        /// <summary>
        /// service collection
        /// </summary>
        IServiceCollection Services { get; }
    }
}
