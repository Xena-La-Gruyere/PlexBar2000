using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Interface.UIHelper;
using PlexClient.Library.Models;
using ReactiveUI;
using Splat;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour AlbumView.xaml
    /// </summary>
    public partial class AlbumView : ReactiveUserControl<MainViewModel>
    {
        public AlbumView()
        {
            InitializeComponent();


            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                PlayButton.Events().PreviewMouseLeftButtonDown
                    .Throttle(TimeSpan.FromSeconds(1))
                    .WithLatestFrom(ViewModel.Album, (_, a) => a)
                    .ObserveOnDispatcher()
                    .Subscribe(ViewModel.PlayAlbum)
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.Title)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(title => AlbumTitle.Text = title)
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.Year)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(year => AlbumYear.Text = year.ToString())
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => $"{a?.Tracks.FirstOrDefault()?.Codec.ToUpper()} {a?.Tracks.FirstOrDefault()?.Bitrate} kbps")
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(codeBitrate => CodecBitrate.Text = codeBitrate)
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.ThumbnailUrl)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(url => AlbumThumbnailView.Thumbnail.Source = url is null ? null : new BitmapImage(url))
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.Tracks)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(tracks => TrackList.ItemsSource = tracks)
                    .DisposeWith(dispose);
            });
        }
        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }

        private void ClickTrack(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Album.Take(1).Subscribe(album =>
            {
                if (e.LeftButton == MouseButtonState.Pressed &&
                    sender is FrameworkElement fe &&
                    fe.DataContext is TrackModel track)
                    ViewModel.PlayTrackAndAlbum(album, track);
            });
        }
    }
}
