using Microsoft.Extensions.DependencyInjection;

namespace heitech.configXt.Application.Components
{
    public static class BusComponent
    {
        ///<summary>
        /// Create a RequestBus that uses Microsoft.Extensions.DependencyInjection 
        ///</summary>
        public static IServiceCollection RegisterRequestBus(this IServiceCollection services, string connection)
        {
            services.AddSingleton<IRequestBus>(new RequestBus(connection));
            return services;
        }

        ///<summary>
        /// Create a ResponseBus that uses Microsoft.Extensions.DependencyInjection 
        ///</summary>
        public static IServiceCollection RegisterResponseBus(this IServiceCollection services, string connection)
        {
            services.AddSingleton<IResponseBus>(new ResponseBus(connection));
            return services;
        }
    }
}