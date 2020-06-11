using System;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using ApplicationState.States;
using Interface.Styles.Converters;
using Interface.UIHelper;
using ReactiveUI;
using Splat;
using EventExtensions = System.Windows.Controls.Primitives.EventExtensions;

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
                ViewModel.PlayingTrack
                    .Select(p => p?.ThumbnailUrl)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(url => PlayingAlbumThumbnail.Source = url is null ? null : new BitmapImage(url))
                    .DisposeWith(dispose);

                ViewModel.PlayingTrack
                    .Select(p => p?.Artist)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(a => ArtistName.Text = a)
                    .DisposeWith(dispose);

                ViewModel.PlayingTrack
                    .Select(p => p?.Album)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(a => AlbumName.Text = a)
                    .DisposeWith(dispose);

                ViewModel.PlayingTrack
                    .Select(p => p?.Title)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(a => TrackName.Text = a)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.PlayingState)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(s => PausedBorder.Visibility = s == PlayingStateEnum.Playing ? 
                        Visibility.Collapsed : Visibility.Visible)
                    .DisposeWith(dispose);

                ResumePauseButton.Events()
                    .MouseWheel
                    .Where(e => e.Delta > 0)
                    .Subscribe(ViewModel.WheelUp)
                    .DisposeWith(dispose);
                ResumePauseButton.Events()
                    .MouseWheel
                    .Where(e => e.Delta < 0)
                    .Subscribe(ViewModel.WheelDown)
                    .DisposeWith(dispose);

                ResumePauseButton.Events()
                    .PreviewMouseLeftButtonDown
                    .WithLatestFrom(ViewModel.PlayingTrack, (args, track) => new { args, track })
                    .Where(e => e.track != null)
                    .Select(e => e.args)
                    .Throttle(TimeSpan.FromMilliseconds(100))
                    .ObserveOnDispatcher()
                    .Subscribe(ViewModel.PauseResume)
                    .DisposeWith(dispose);

                var converterMilliSec = new MilliSecConverter();

                ViewModel.PlayingTrack
                    .Select(p => p?.Duration)
                    .DistinctUntilChanged()
                    .Select(e => converterMilliSec.Convert(e, typeof(string), null, CultureInfo.InvariantCulture))
                    .Select(e => $"/{e}")
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(e => TrackDuration.Text = e)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => (long)p.Avancement.TotalMilliseconds)
                    .Select(e => converterMilliSec.Convert(e, typeof(string), null, CultureInfo.InvariantCulture))
                    .Select(e => e?.ToString())
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(e => TrackAvancement.Text = e)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.Avancement.TotalMilliseconds)
                    .WithLatestFrom(ViewModel.PlayingTrack.Select(p => p?.Duration),
                        (pos, lenght) => pos / lenght)
                    .Where(e => e.HasValue)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(e => TimeLineElapsed.Width = TimeLine.ActualWidth * e.Value)
                    .DisposeWith(dispose);

                ViewModel.Player
                    .Select(p => p.VolumentPercentage)
                    .DistinctUntilChanged()
                    .ObserveOnDispatcher()
                    .Subscribe(e =>
                    {
                        Volume.Height = e;

                        var animation = Grid.FindResource("VolumeAnimation") as Storyboard;
                        Volume.BeginStoryboard(animation);
                    })
                    .DisposeWith(dispose);
            });
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }
    }
}
