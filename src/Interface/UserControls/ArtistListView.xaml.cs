using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DynamicData;
using Interface.UIHelper;
using PlexClient.Library.Models;
using ReactiveUI;
using Splat;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ArtistListView.xaml
    /// </summary>
    public partial class ArtistListView : ReactiveUserControl<MainViewModel>
    {
        public ArtistListView()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                ViewModel.Artists
                    .ObserveOnDispatcher()
                    .Subscribe(artists => ArtistList.ItemsSource = artists)
                    .DisposeWith(dispose);

                ViewModel.Letters
                    .ObserveOnDispatcher()
                    .Subscribe(letters => LetterList.ItemsSource = letters)
                    .DisposeWith(dispose);
            });
        }

        private void ArtistOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
               button.DataContext is ArtistModel artist)
                ViewModel.ClickArtist(artist);
        }

        private void LetterMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is TextBlock textBlock) || !(textBlock.DataContext is char letter)) return;

            var scrollViewer = UiHelper.FindChild<ScrollViewer>(Application.Current.MainWindow, "ScrollViewer");

            var artists = ArtistList.ItemsSource.Cast<ArtistModel>().ToArray();

            var firstArtist = artists.First(a => a.LetterSearch == letter);

            scrollViewer.ScrollToVerticalOffset(artists.IndexOf(firstArtist));
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }
    }
}
