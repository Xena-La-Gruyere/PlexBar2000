﻿using Microsoft.Extensions.DependencyInjection;

namespace ApplicationState
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddApplicationStateService(this IServiceCollection builder)
        {
            return builder.AddSingleton<IApplicationStateService, ApplicationStateService>();
        }
    }
}
