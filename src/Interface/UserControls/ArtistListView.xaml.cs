using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
