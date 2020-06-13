using Microsoft.Extensions.DependencyInjection;

namespace MediaSession
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddMediaSession(this IServiceCollection builder)
        {
            return builder.AddHostedService<MediaSessionService>();
        }
    }
}
