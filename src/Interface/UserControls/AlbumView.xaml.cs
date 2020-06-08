using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interface.UIHelper;
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
                ViewModel.Album
                    .Select(a => a?.Title)
                    .DistinctUntilChanged()
                    .Subscribe(title => AlbumTitle.Text = title)
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.Year)
                    .DistinctUntilChanged()
                    .Subscribe(year => AlbumYear.Text = year.ToString())
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => $"{a?.Tracks.FirstOrDefault()?.Codec.ToUpper()} {a?.Tracks.FirstOrDefault()?.Bitrate} kbps")
                    .DistinctUntilChanged()
                    .Subscribe(codeBitrate => CodecBitrate.Text = codeBitrate)
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.ThumbnailUrl)
                    .DistinctUntilChanged()
                    .Subscribe(url => Thumbnail.Source = url is null ? null : new BitmapImage(new Uri(url)))
                    .DisposeWith(dispose);

                ViewModel.Album
                    .Select(a => a?.Tracks)
                    .DistinctUntilChanged()
                    .Subscribe(tracks => TrackList.ItemsSource = tracks)
                    .DisposeWith(dispose);
            });
        }
        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }
    }
}
