using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ApplicationState;
using ColorThiefDotNet;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;

namespace ThemeColorManager
{
    public class ImageTheme : IImageTheme
    {
        public IObservable<Brush> Primary { get; }
        public IObservable<Brush> Background { get; }
        public IObservable<BitmapSource> Image { get; }

        private readonly BehaviorSubject<Brush> _primary;
        private readonly BehaviorSubject<Brush> _background;
        private readonly BehaviorSubject<BitmapSource> _image;
        private readonly Subject<BitmapSource> _loadedImage;
        private readonly ColorThief _colorThief;

        public ImageTheme(IApplicationStateService applicationState)
        {
            _colorThief = new ColorThief();
            _loadedImage = new Subject<BitmapSource>();
            _image = new BehaviorSubject<BitmapSource>(null);
            _background = new BehaviorSubject<Brush>(new SolidColorBrush(Colors.Black) {Opacity = 0.6f});
            _primary = new BehaviorSubject<Brush>(new SolidColorBrush(Colors.WhiteSmoke));

            Background = _background;
            Primary = _primary;
            Image = _image;

            applicationState.PlayingTrack
                .Select(p => p?.ThumbnailUrl)
                .DistinctUntilChanged()
                .Subscribe(url =>
                {
                    if (_image.Value != null)
                        _image.Value.DownloadCompleted -= ValueOnDownloadCompleted;
                    
                    _image.OnNext(url is null ? null : new BitmapImage(url));
                });

            _image
                .Subscribe(thumb =>
                {
                    if(thumb is null)
                        _loadedImage.OnNext(null);
                    else
                        thumb.DownloadCompleted += ValueOnDownloadCompleted;
                });

            var palette = _loadedImage
                .Select(img => Observable.FromAsync(() => GetBitmap(img)))
                .Concat()
                .Select(GetPalette)
                .Replay();

            palette
                .ObserveOnDispatcher()
                .Select(GetPrimary)
                .Subscribe(p => _primary.OnNext(p));
            palette
                .ObserveOnDispatcher()
                .Select(GetBackground)
                .Subscribe(b => _background.OnNext(b));

            palette.Connect();
        }

        async Task<Bitmap> GetBitmap(BitmapSource source)
        {
            if (source is null) return null;

            var clone = source.Clone();
            clone.Freeze();

            return await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(
                    clone.PixelWidth,
                    clone.PixelHeight,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                BitmapData data = bmp.LockBits(
                    new Rectangle(Point.Empty, bmp.Size),
                    ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                clone.CopyPixels(
                    Int32Rect.Empty,
                    data.Scan0,
                    data.Height * data.Stride,
                    data.Stride);

                bmp.UnlockBits(data);
                return bmp;
            });
        }

        private List<QuantizedColor> GetPalette(Bitmap bitmap)
            => bitmap is null ? null : _colorThief.GetPalette(bitmap);

        private Color GetColorFromThief(QuantizedColor color)
            => Color.FromRgb(color.Color.R, color.Color.G, color.Color.B);

        private byte DarkenCanal(byte canal)
            => canal - 20 > 0 ? (byte)(canal - 20) : canal;

        private Color GetBackgroundColorFromThief(QuantizedColor color)
            => Color.FromArgb(153,
                DarkenCanal(color.Color.R),
                DarkenCanal(color.Color.G),
                DarkenCanal(color.Color.B));
        

        private Brush GetPrimary(List<QuantizedColor> colors)
        {
            var bestColor = colors?.FirstOrDefault(c => !c.IsDark);

            var color = bestColor != null
                ? GetColorFromThief(bestColor)
                : Colors.WhiteSmoke;

            return new SolidColorBrush(color);
        }

        private Brush GetBackground(List<QuantizedColor> colors)
        {
            var bestColor = colors?.FirstOrDefault(c => c.IsDark);

            var secondBestColor = colors?.LastOrDefault(c => c.IsDark);

            var color = bestColor != null
                ? GetBackgroundColorFromThief(bestColor)
                : Colors.Black;

            if (secondBestColor != null)
            {
                return new LinearGradientBrush(color, GetBackgroundColorFromThief(secondBestColor), 90);
            }
            return new SolidColorBrush(color);
        }

        private void ValueOnDownloadCompleted(object sender, EventArgs e)
        {
            if (sender is BitmapSource bitmapSource)
                _loadedImage.OnNext(bitmapSource);
        }
    }
}
