using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Interface.UIHelper;
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
                    .Subscribe(abums => AlbumList.ItemsSource = abums)
                    .DisposeWith(dispose);
            });
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            e.DragMoveWindow(this);
        }
    }
}
