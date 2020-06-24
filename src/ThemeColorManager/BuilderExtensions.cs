using Microsoft.Extensions.DependencyInjection;

namespace ThemeColorManager
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddImageTheme(this IServiceCollection builder)
        {
            return builder.AddSingleton<IImageTheme, ImageTheme>();
        }
    }
}
