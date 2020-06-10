using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using ReactiveUI;
using Splat;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour LibraryView.xaml
    /// </summary>
    public partial class LibraryView : ReactiveUserControl<MainViewModel>
    {
        public LibraryView()
        {
            InitializeComponent(); ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                ButtonPrevious.Events()
                    .PreviewMouseLeftButtonDown
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .ObserveOnDispatcher()
                    .Subscribe(ViewModel.ClickPrevious)
                    .DisposeWith(dispose);
                HomeButton.Events()
                    .PreviewMouseLeftButtonDown
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .ObserveOnDispatcher()
                    .Subscribe(ViewModel.HomeButton)
                    .DisposeWith(dispose);
                PlaylistButton.Events()
                    .PreviewMouseLeftButtonDown
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .ObserveOnDispatcher()
                    .Subscribe(ViewModel.PlaylistButton)
                    .DisposeWith(dispose);

                ViewModel.MenuIndex
                    .ObserveOnDispatcher()
                    .Subscribe(ind => TransitionerMenu.SelectedIndex = ind)
                    .DisposeWith(dispose);
            });
        }
    }
}
