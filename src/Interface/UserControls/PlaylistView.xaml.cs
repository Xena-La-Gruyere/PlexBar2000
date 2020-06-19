using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Interface.UIHelper;
using Library.Abstractions.Models;
using ReactiveUI;
using Splat;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : ReactiveUserControl<MainViewModel>
    {
        public PlaylistView()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                ViewModel.PlaylistAlbum
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(abums => AlbumList.ItemsSource = abums)
                    .DisposeWith(dispose);
            });
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }

        private void ClearAlbum(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is FrameworkElement fe &&
                fe.DataContext is AlbumModel album)
                ViewModel.RemoveAlbumButton(album);
        }

        private void ClickTrack(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is FrameworkElement fe &&
                fe.DataContext is TrackModel track)
                ViewModel.PlayTrackInPlaylist(track);
        }
    }
}
