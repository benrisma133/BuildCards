using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BuildCards.Controls.Components
{
    public enum BadgeVariant
    {
        Neutral,
        Success,
        Warning,
        Danger,
        Info
    }

    public partial class Badge : UserControl
    {
        public Badge()
        {
            InitializeComponent();
            UpdateAppearance();
        }

        // 1. خاصية النص (Text)
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(Badge),
                new PropertyMetadata("Badge"));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // 2. خاصية النوع/اللون (Variant)
        public static readonly DependencyProperty VariantProperty =
            DependencyProperty.Register(
                nameof(Variant),
                typeof(BadgeVariant),
                typeof(Badge),
                new PropertyMetadata(BadgeVariant.Neutral, OnVisualPropertyChanged));

        public BadgeVariant Variant
        {
            get => (BadgeVariant)GetValue(VariantProperty);
            set => SetValue(VariantProperty, value);
        }

        // 3. خاصية النقطة اللي حدا النص (ShowDot)
        public static readonly DependencyProperty ShowDotProperty =
            DependencyProperty.Register(
                nameof(ShowDot),
                typeof(bool),
                typeof(Badge),
                new PropertyMetadata(false, OnVisualPropertyChanged));

        public bool ShowDot
        {
            get => (bool)GetValue(ShowDotProperty);
            set => SetValue(ShowDotProperty, value);
        }

        // 4. خاصية استدارة الحواف (CornerRadius)
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(Badge),
                new PropertyMetadata(new CornerRadius(12)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Badge badge)
            {
                badge.UpdateAppearance();
            }
        }

        private void UpdateAppearance()
        {
            StatusDot.Visibility = ShowDot ? Visibility.Visible : Visibility.Collapsed;

            // ألوان عصرية (Soft background + Crisp text + Subtle border)
            var (bg, border, fg) = Variant switch
            {
                BadgeVariant.Success => ("#ECFDF5", "#A7F3D0", "#065F46"), // أخضر
                BadgeVariant.Warning => ("#FFFBEB", "#FDE68A", "#92400E"), // أصفر/برتقالي
                BadgeVariant.Danger => ("#FEF2F2", "#FECACA", "#991B1B"), // أحمر
                BadgeVariant.Info => ("#EFF6FF", "#BFDBFE", "#1E40AF"), // أزرق
                _ => ("#F3F4F6", "#E5E7EB", "#374151")  // رمادي (Neutral)
            };

            BadgeBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(bg)!;
            BadgeBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(border)!;

            var foregroundBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(fg)!;
            BadgeText.Foreground = foregroundBrush;
            StatusDot.Fill = foregroundBrush;
        }
    }
}