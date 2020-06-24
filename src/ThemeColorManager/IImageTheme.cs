using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ThemeColorManager
{
    public interface IImageTheme
    {
        IObservable<Brush> Primary { get; }
        IObservable<Brush> Background { get; }
        IObservable<BitmapSource> Image { get; }
    }
}
