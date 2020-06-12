using System;
using System.Windows;
using System.Windows.Controls;

namespace Interface.UserControls
{
    /// <summary>
    /// Logique d'interaction pour AlbumThumbnailView.xaml
    /// </summary>
    public partial class AlbumThumbnailView : UserControl
    {
        public AlbumThumbnailView()
        {
            InitializeComponent();

            Thumbnail.DataContext = this;
        }

        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(AlbumThumbnailView), new PropertyMetadata(null));
    }
}
