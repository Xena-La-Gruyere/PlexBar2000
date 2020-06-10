using System;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Interface.Styles.Converters;
using Interface.UIHelper;
using ReactiveUI;
using Splat;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour PlayerView.xaml
    /// </summary>
    public partial class PlayerView : ReactiveUserControl<MainViewModel>
    {
        public PlayerView()
        {
            InitializeComponent(); 

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                ViewModel.Player
                    .Select(p => p.PlayingTrack?.ThumbnailUrl)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(url => PlayingAlbumThumbnail.Source = url is null ? null : new BitmapImage(new Uri(url)))
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.PlayingTrack?.Artist)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(a => ArtistName.Text = a)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.PlayingTrack?.Album)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(a => AlbumName.Text = a)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.PlayingTrack?.Title)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(a => TrackName.Text = a)
                    .DisposeWith(dispose);

                var converterMilliSec = new MilliSecConverter();

                ViewModel.Player
                    .Select(p => p.PlayingTrack?.Duration)
                    .Select(e => converterMilliSec.Convert(e, typeof(string), null, CultureInfo.InvariantCulture))
                    .Select(e => $"/{e}")
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(e => TrackDuration.Text = e)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.Avancement)
                    .Select(e => converterMilliSec.Convert(e, typeof(string), null, CultureInfo.InvariantCulture))
                    .Select(e => e?.ToString())
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(e => TrackAvancement.Text = e)
                    .DisposeWith(dispose);

            });
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }
    }
}
