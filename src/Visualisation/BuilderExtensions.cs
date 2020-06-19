using Microsoft.Extensions.DependencyInjection;

namespace Visualisation
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddVisualisation(this IServiceCollection builder)
        {
            return builder.AddSingleton<SpectrumProvider>();
        }
    }
}
