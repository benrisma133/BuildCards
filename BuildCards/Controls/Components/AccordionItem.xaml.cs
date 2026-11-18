using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace BuildCards.Controls.Components
{
    [ContentProperty(nameof(BodyContent))]
    public partial class AccordionItem : UserControl
    {
        public AccordionItem()
        {
            InitializeComponent();
            Loaded += (_, _) => ApplyVisualState(animate: false);
        }

        // 1. Header
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(string),
                typeof(AccordionItem),
                new PropertyMetadata("Section Title"));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        // 2. Subtitle
        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(
                nameof(Subtitle),
                typeof(string),
                typeof(AccordionItem),
                new PropertyMetadata(string.Empty, OnMetadataChanged));

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        // 3. BadgeText
        public static readonly DependencyProperty BadgeTextProperty =
            DependencyProperty.Register(
                nameof(BadgeText),
                typeof(string),
                typeof(AccordionItem),
                new PropertyMetadata(string.Empty, OnMetadataChanged));

        public string BadgeText
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }

        // 4. BadgeVariant
        public static readonly DependencyProperty BadgeVariantProperty =
            DependencyProperty.Register(
                nameof(BadgeVariant),
                typeof(BadgeVariant),
                typeof(AccordionItem),
                new PropertyMetadata(BadgeVariant.Neutral));

        public BadgeVariant BadgeVariant
        {
            get => (BadgeVariant)GetValue(BadgeVariantProperty);
            set => SetValue(BadgeVariantProperty, value);
        }

        // 5. IsExpanded (TwoWay by default for clean MVVM binding)
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(
                nameof(IsExpanded),
                typeof(bool),
                typeof(AccordionItem),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnIsExpandedChanged));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        // 6. BodyContent (Default ContentProperty)
        public static readonly DependencyProperty BodyContentProperty =
            DependencyProperty.Register(
                nameof(BodyContent),
                typeof(object),
                typeof(AccordionItem),
                new PropertyMetadata(null));

        public object BodyContent
        {
            get => GetValue(BodyContentProperty);
            set => SetValue(BodyContentProperty, value);
        }

        private static void OnMetadataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AccordionItem item)
            {
                item.SubtitleText.Visibility = string.IsNullOrWhiteSpace(item.Subtitle)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                item.HeaderBadge.Visibility = string.IsNullOrWhiteSpace(item.BadgeText)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AccordionItem item)
            {
                item.ApplyVisualState(animate: item.IsLoaded);
            }
        }

        private void Header_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void ApplyVisualState(bool animate)
        {
            double targetAngle = IsExpanded ? 180.0 : 0.0;
            double targetScale = IsExpanded ? 1.0 : 0.0;

            if (!animate)
            {
                ChevronRotate.Angle = targetAngle;
                BodyScale.ScaleY = targetScale;
                BodyContainer.Visibility = IsExpanded ? Visibility.Visible : Visibility.Collapsed;
                return;
            }

            var duration = TimeSpan.FromMilliseconds(200);
            var easing = new CubicEase { EasingMode = EasingMode.EaseOut };

            // Rotate chevron
            var rotateAnim = new DoubleAnimation(targetAngle, duration) { EasingFunction = easing };
            ChevronRotate.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, rotateAnim);

            // Expand or collapse body
            if (IsExpanded)
            {
                BodyContainer.Visibility = Visibility.Visible;
            }

            var scaleAnim = new DoubleAnimation(targetScale, duration) { EasingFunction = easing };
            scaleAnim.Completed += (_, _) =>
            {
                if (!IsExpanded)
                {
                    BodyContainer.Visibility = Visibility.Collapsed;
                }
            };

            BodyScale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleAnim);
        }
    }
}