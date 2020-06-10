using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Interface.Styles.Decorators
{
    public class RippleEffectDecorator : ContentControl
    {
        static RippleEffectDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RippleEffectDecorator),
                new FrameworkPropertyMetadata(typeof(RippleEffectDecorator)));
        }

        public Brush HighlightBackground
        {
            get => (Brush) GetValue(HighlightBackgroundProperty);
            set => SetValue(HighlightBackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighlightBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(RippleEffectDecorator),
                new PropertyMetadata(Brushes.White));


        public bool RipppleCenter
        {
            get => (bool)GetValue(RipppleCenterProperty);
            set => SetValue(RipppleCenterProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighlightBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RipppleCenterProperty =
            DependencyProperty.Register("RipppleCenter", typeof(bool), typeof(RippleEffectDecorator), new PropertyMetadata(false));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Ellipse ellipse = GetTemplateChild("PART_ellipse") as Ellipse;
            Grid grid = GetTemplateChild("PART_grid") as Grid;
            Storyboard animation = grid.FindResource("PART_animation") as Storyboard;

            this.AddHandler(MouseDownEvent, new RoutedEventHandler((sender, e) =>
            {
                var targetWidth = RipppleCenter ?
                    Math.Min(ActualWidth, ActualHeight) :
                    Math.Max(ActualWidth, ActualHeight) * 2;
                var startPosition = RipppleCenter ? 
                    new Point(ActualWidth / 2, ActualHeight / 2) :
                    (e as MouseButtonEventArgs).GetPosition(this);
                var startMargin = new Thickness(startPosition.X, startPosition.Y, 0, 0);
                //set initial margin to mouse position
                ellipse.Margin = startMargin;
                //set the to value of the animation that animates the width to the target width
                (animation.Children[0] as DoubleAnimation).To = targetWidth;
                //set the to and from values of the animation that animates the distance relative to the container (grid)
                (animation.Children[1] as ThicknessAnimation).From = startMargin;
                (animation.Children[1] as ThicknessAnimation).To = new Thickness(startPosition.X - targetWidth / 2,
                    startPosition.Y - targetWidth / 2, 0, 0);
                ellipse.BeginStoryboard(animation);
            }), true);
        }
    }
}
