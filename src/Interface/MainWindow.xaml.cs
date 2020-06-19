using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using ApplicationState.Enumerations;
using Interface.UIHelper;
using ReactiveUI;
using Splat;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
                {
                    //this.Events().KeyDown
                    //    .Where(key => key.Key == Key.MediaPlayPause)
                    //    .Throttle(TimeSpan.FromMilliseconds(100))
                    //    .ObserveOnDispatcher()
                    //    .Subscribe(_ => ViewModel.PauseResume())
                    //    .DisposeWith(dispose);
                    //this.Events().KeyDown
                    //    .Where(key => key.Key == Key.MediaNextTrack)
                    //    .ObserveOnDispatcher()
                    //    .Subscribe(_ => ViewModel.PlayNext())
                    //    .DisposeWith(dispose);
                    //this.Events().KeyDown
                    //    .Where(key => key.Key == Key.MediaPreviousTrack)
                    //    .ObserveOnDispatcher()
                    //    .Subscribe(_ => ViewModel.PlayPrevious())
                    //    .DisposeWith(dispose);
                    //this.Events().KeyDown
                    //    .Where(key => key.Key == Key.MediaStop)
                    //    .ObserveOnDispatcher()
                    //    .Subscribe(_ => ViewModel.ClearPlaylist())
                    //    .DisposeWith(dispose);

                    this.Events().MouseDown
                        .Where(e => e.MiddleButton == MouseButtonState.Pressed)
                        .ObserveOnDispatcher()
                        .Subscribe(ViewModel.MiddleMouseClick)
                        .DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Player)
                        .ObserveOnDispatcher()
                        .Subscribe(x =>
                        {
                            Width = 300;
                            Height = 100;
                            LibraryView.Visibility = Visibility.Collapsed;
                            PlayerView.Visibility = Visibility.Visible;
                        }).DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Explorer)
                        .ObserveOnDispatcher()
                        .Subscribe(x =>
                        {
                            Width = 500;
                            Height = 600;
                            LibraryView.Visibility = Visibility.Visible;
                            PlayerView.Visibility = Visibility.Collapsed;
                        }).DisposeWith(dispose);
                }
            );
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.EnableBlurBehind();

        }
    }
}
