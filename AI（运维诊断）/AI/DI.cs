using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace AI

{

    public static class Di

    {

        public static IServiceCollection Services { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

    }

    public static class Extensions

    {

        public static IServiceCollection AddTfDi(this IServiceCollection services)

        {

            Di.Services = services;

            return services;

        }

        public static IApplicationBuilder UseTfDi(this IApplicationBuilder builder)

        {

            Di.ServiceProvider = builder.ApplicationServices;

            return builder;

        }

    }

}