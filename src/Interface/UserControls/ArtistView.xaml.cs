using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Interface.UIHelper;
using PlexClient.Library.Models;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ArtistView.xaml
    /// </summary>
    public partial class ArtistView : ReactiveUserControl<MainViewModel>
    {
        public ArtistView()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                ViewModel.Artist
                    .Select(a => a?.Title)
                    .DistinctUntilChanged()
                    .Subscribe(title => ArtistTitle.Text = title)
                    .DisposeWith(dispose);

                ViewModel.Artist
                    .Select(a => a?.ThumbnailUrl)
                    .DistinctUntilChanged()
                    .Subscribe(url => Thumbnail.Source = url is null ? null : new BitmapImage(new Uri(url)))
                    .DisposeWith(dispose);

                ViewModel.Artist
                    .Select(a => a?.Albums)
                    .DistinctUntilChanged()
                    .Subscribe(abums => AlbumList.ItemsSource = abums)
                    .DisposeWith(dispose);
            });
        }
        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }

        private void AlbumClick(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed) return;
            if (sender is Grid grid &&
                grid.DataContext is AlbumModel album)
                ViewModel.ClickAlbum(album);
        }
    }
}
