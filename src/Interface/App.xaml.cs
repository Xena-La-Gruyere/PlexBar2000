using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using ApplicationState;
using MediaSession;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Player;
using PlexClient;
using ReactiveUI;
using Splat;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Interface
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        private MainWindow _window;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(ConfigureLogger);
            _host = hostBuilder.Build();
            ReactiveInjectionExtensions.Provider = _host.Services;

            _window = _host.Services.GetRequiredService<MainWindow>();
            _window.Closing += MainWindowOnClosing;

            _host.RunAsync();

            _window.Show();
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs e)
        {
            ShutDown().GetAwaiter().GetResult();
        }

        private async Task ShutDown()
        {
            await _host.StopAsync();

            Current.Shutdown();
        }

        private void ConfigureLogger(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddDebug();

#if DEBUG
            loggingBuilder.SetMinimumLevel(LogLevel.Debug);
#endif
#if RELEASE
            loggingBuilder.SetMinimumLevel(Interface.Properties.Settings.Default.Debug ? LogLevel.Debug : LogLevel.Information);
#endif
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Locator.CurrentMutable.Register(() => new CustomPropertyResolver(), typeof(ICreatesObservableForProperty));

            services.AddViewModel<MainWindow, MainViewModel>()
                .AddApplicationStateService()
                .AddPlexLibrary("http://server:32400", "zNk53Ki7BqR4EraZevvP")
                .AddCSCorePlayer()
                .AddMediaSession();
        }
    }


    static class ReactiveInjectionExtensions
    {
        public static IServiceProvider Provider;
        public static IServiceCollection AddViewModel<TView, TViewModel>(this IServiceCollection serviceCollection)
            where TViewModel : ReactiveObject
            where TView : ReactiveWindow<TViewModel>
        {
            serviceCollection.AddSingleton<TView>();
            serviceCollection.AddSingleton<TViewModel>();

            Locator.CurrentMutable.Register(() => Provider.GetRequiredService<TViewModel>(), typeof(TViewModel));

            return serviceCollection;
        }
    }

    public class CustomPropertyResolver : ICreatesObservableForProperty
    {
        public int GetAffinityForObject(Type type, string propertyName, bool beforeChanged = false)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(type))
                return 0;
            var fi = type.GetTypeInfo().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .FirstOrDefault(x => x.Name == propertyName);

            return fi != null ? 2 /* POCO affinity+1 */ : 0;
        }

        public IObservable<IObservedChange<object, object>> GetNotificationForProperty(object sender, System.Linq.Expressions.Expression expression, string propertyName,
            bool beforeChanged = false, bool suppressWarnings = false)
        {
            var foo = (FrameworkElement)sender;
            return Observable.Return(new ObservedChange<object, object>(sender, expression), new DispatcherScheduler(foo.Dispatcher))
                .Concat(Observable.Never<IObservedChange<object, object>>());
        }
    }
}
