using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ApplicationState.Enumerations;
using DynamicData;
using PlexClient.Client.Models;
using PlexClient.Library.Models;
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

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
                {
                    this.Events().MouseDown
                        .Where(e => e.MiddleButton == MouseButtonState.Pressed)
                        .Subscribe(ViewModel.MiddleMouseClick)
                        .DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Player)
                        .Subscribe(x =>
                        {
                            Width = 300;
                            Height = 100;
                        }).DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Explorer)
                        .Subscribe(x =>
                        {
                            Width = 500;
                            Height = 600;
                        }).DisposeWith(dispose);

                    ViewModel.Artists
                        .ObserveOnDispatcher()
                        .Subscribe(artists => ArtistList.ItemsSource = artists)
                        .DisposeWith(dispose);

                    ViewModel.Letters
                        .ObserveOnDispatcher()
                        .Subscribe(letters => LetterList.ItemsSource = letters)
                        .DisposeWith(dispose);
                }
            );
        }


        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
            e.Handled = false;
        }

        private void Letter_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button textBlock &&
                textBlock.DataContext is char charSearch)
            {
                
                
            }

        }

        private void ArtistInitialized(object sender, EventArgs e)
        {

        }
    }
}
