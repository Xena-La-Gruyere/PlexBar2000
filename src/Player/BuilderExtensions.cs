using Microsoft.Extensions.DependencyInjection;

namespace Player
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddCSCorePlayer(this IServiceCollection builder)
        {
            return builder.AddHostedService<PlayerService>();
        }
    }
}
