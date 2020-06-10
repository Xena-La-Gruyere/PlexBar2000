﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ApplicationState.Enumerations;
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
                        .ObserveOnDispatcher()
                        .Subscribe(ViewModel.MiddleMouseClick)
                        .DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Player)
                        .ObserveOnDispatcher()
                        .Subscribe(x =>
                        {
                            Width = 300;
                            Height = 100;
                            LibraryView.Visibility = Visibility.Collapsed;
                        }).DisposeWith(dispose);

                    ViewModel.AppState
                        .Where(s => s == AppStateEnum.Explorer)
                        .ObserveOnDispatcher()
                        .Subscribe(x =>
                        {
                            Width = 500;
                            Height = 600;
                            LibraryView.Visibility = Visibility.Visible;
                        }).DisposeWith(dispose);
                }
            );
        }


    }
}
