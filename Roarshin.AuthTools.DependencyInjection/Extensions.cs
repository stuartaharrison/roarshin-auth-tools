using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Roarshin.AuthTools.DependencyInjection {
    
    public static class Extensions {
    
        public static IServiceCollection AddRoarshinAuthTools<T>(this IServiceCollection services, Action<RoarshinAuthToolOptions> config) {
            // register the RoarshinAuthToolOptions configuration/options
            services.Configure(config);

            services.AddSingleton<IPasswordGenerator, PasswordHandler>(opt => {
                var options = opt.GetRequiredService<IOptions<RoarshinAuthToolOptions>>();
                return new PasswordHandler(options.Value.KeySize, options.Value.SaltSize, options.Value.Iterations, options.Value.ValidCharacters);
            });

            services.AddSingleton<IPasswordVerifier, PasswordHandler>(opt => {
                var options = opt.GetRequiredService<IOptions<RoarshinAuthToolOptions>>();
                return new PasswordHandler(options.Value.KeySize, options.Value.SaltSize, options.Value.Iterations, options.Value.ValidCharacters);
            });

            services.AddSingleton<ITokenSigner<T>, TokenHandler<T>>(opt => {
                var options = opt.GetRequiredService<IOptions<RoarshinAuthToolOptions>>();
                return new TokenHandler<T>(options.Value.TokenKey, options.Value.TokenExpiresInMinutes);
            });

            services.AddSingleton<ITokenVerifier<T>, TokenHandler<T>>(opt => {
                var options = opt.GetRequiredService<IOptions<RoarshinAuthToolOptions>>();
                return new TokenHandler<T>(options.Value.TokenKey, options.Value.TokenExpiresInMinutes);
            });

            return services;
        }   
    }
}
