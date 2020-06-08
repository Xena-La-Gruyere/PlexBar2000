using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DynamicData;
using PlexClient.Library.Models;
using ReactiveUI;
using Splat;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour ArtistListView.xaml
    /// </summary>
    public partial class ArtistListView : ReactiveUserControl<MainViewModel>
    {
        public ArtistListView()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(dispose =>
            {
                ViewModel.Artists
                    .ObserveOnDispatcher()
                    .Subscribe(artists => ArtistList.ItemsSource = artists)
                    .DisposeWith(dispose);

                ViewModel.Letters
                    .ObserveOnDispatcher()
                    .Subscribe(letters => LetterList.ItemsSource = letters)
                    .DisposeWith(dispose);
            });
        }

        private void ArtistOnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
               button.DataContext is ArtistModel artist)
                ViewModel.ClickArtist(artist);
        }

        private void LetterMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is TextBlock textBlock) || !(textBlock.DataContext is char letter)) return;

            var scrollViewer = FindChild<ScrollViewer>(Application.Current.MainWindow, "ScrollViewer");

            var artists = ArtistList.ItemsSource.Cast<ArtistModel>().ToArray();

            var firstArtist = artists.First(a => a.LetterSearch == letter);

            scrollViewer.ScrollToVerticalOffset(artists.IndexOf(firstArtist));
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
