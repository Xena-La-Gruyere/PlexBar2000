using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ApplicationState.Enumerations;
using MaterialDesignThemes.Wpf.Transitions;
using ReactiveUI;
using Splat;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
                {
                    this.Events().MouseDown
                        .Where(e => e.MiddleButton == MouseButtonState.Pressed)
                        .Subscribe(ViewModel.MiddleMouseClick)
                        .DisposeWith(dispose);

                    ButtonPrevious.Events()
                        .PreviewMouseLeftButtonDown
                        .Subscribe(ViewModel.ClickPrevious)
                        .DisposeWith(dispose);

                    ViewModel.MenuIndex
                        .Subscribe(ind => TransitionerMenu.SelectedIndex = ind)
                        .DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Player)
                        .Subscribe(x =>
                        {
                            Width = 300;
                            Height = 100;
                        }).DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Explorer)
                        .Subscribe(x =>
                        {
                            Width = 500;
                            Height = 600;
                        }).DisposeWith(dispose);
                }
            );
        }


    }
}
