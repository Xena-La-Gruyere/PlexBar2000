using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
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
