using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BuildCards.Controls.Components
{
    public partial class StatCard : UserControl
    {
        public StatCard()
        {
            InitializeComponent();
            UpdateVisualState();
        }

        // 1. Title
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(StatCard),
                new PropertyMetadata("Metric Title"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // 2. Hero Value
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(string),
                typeof(StatCard),
                new PropertyMetadata("0"));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // 3. Subtitle
        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(
                nameof(Subtitle),
                typeof(string),
                typeof(StatCard),
                new PropertyMetadata(string.Empty));

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        // 4. Trend Text (e.g., "+12.5%")
        public static readonly DependencyProperty TrendTextProperty =
            DependencyProperty.Register(
                nameof(TrendText),
                typeof(string),
                typeof(StatCard),
                new PropertyMetadata(string.Empty, OnVisualPropertyChanged));

        public string TrendText
        {
            get => (string)GetValue(TrendTextProperty);
            set => SetValue(TrendTextProperty, value);
        }

        // 5. IsPositive (switches TrendBadge between Success and Danger)
        public static readonly DependencyProperty IsPositiveProperty =
            DependencyProperty.Register(
                nameof(IsPositive),
                typeof(bool),
                typeof(StatCard),
                new PropertyMetadata(true, OnVisualPropertyChanged));

        public bool IsPositive
        {
            get => (bool)GetValue(IsPositiveProperty);
            set => SetValue(IsPositiveProperty, value);
        }

        // 6. IconData (Vector Geometry)
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register(
                nameof(IconData),
                typeof(Geometry),
                typeof(StatCard),
                new PropertyMetadata(null, OnVisualPropertyChanged));

        public Geometry IconData
        {
            get => (Geometry)GetValue(IconDataProperty);
            set => SetValue(IconDataProperty, value);
        }

        // 7. Optional Icon Accent Tint
        public static readonly DependencyProperty IconAccentBrushProperty =
            DependencyProperty.Register(
                nameof(IconAccentBrush),
                typeof(Brush),
                typeof(StatCard),
                new PropertyMetadata(null, OnVisualPropertyChanged));

        public Brush IconAccentBrush
        {
            get => (Brush)GetValue(IconAccentBrushProperty);
            set => SetValue(IconAccentBrushProperty, value);
        }

        private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StatCard card)
            {
                card.UpdateVisualState();
            }
        }

        private void UpdateVisualState()
        {
            // Toggle TrendBadge visibility and variant
            if (string.IsNullOrWhiteSpace(TrendText))
            {
                TrendBadge.Visibility = Visibility.Collapsed;
            }
            else
            {
                TrendBadge.Visibility = Visibility.Visible;
                TrendBadge.Variant = IsPositive ? BadgeVariant.Success : BadgeVariant.Danger;
            }

            // Toggle Icon visibility
            IconContainer.Visibility = IconData != null ? Visibility.Visible : Visibility.Collapsed;

            if (IconAccentBrush != null)
            {
                IconPath.Fill = IconAccentBrush;
            }
        }
    }
}