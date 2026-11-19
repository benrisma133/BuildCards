using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace BuildCards.Controls.Components
{
    public partial class SegmentedControl : UserControl
    {
        private bool _isSyncing;

        public SegmentedControl()
        {
            InitializeComponent();
            SegmentsHost.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        // 1. ItemsSource
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(SegmentedControl),
                new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // 2. SelectedIndex (TwoWay by default)
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedIndex),
                typeof(int),
                typeof(SegmentedControl),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedIndexChanged));

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        // 3. SelectedItem (TwoWay by default)
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(string),
                typeof(SegmentedControl),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemChanged));

        public string SelectedItem
        {
            get => (string)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // 4. Custom RoutedEvent: SelectionChanged
        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(SelectionChanged),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SegmentedControl));

        public event RoutedEventHandler SelectionChanged
        {
            add => AddHandler(SelectionChangedEvent, value);
            remove => RemoveHandler(SelectionChangedEvent, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SegmentedControl control)
            {
                control.SyncSelectionFromIndex();
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SegmentedControl control && !control._isSyncing)
            {
                control.SyncSelectionFromIndex();
                control.UpdateSegmentVisuals();
                control.RaiseEvent(new RoutedEventArgs(SelectionChangedEvent, control));
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SegmentedControl control && !control._isSyncing)
            {
                control.SyncSelectionFromItem();
                control.UpdateSegmentVisuals();
                control.RaiseEvent(new RoutedEventArgs(SelectionChangedEvent, control));
            }
        }

        private void ItemContainerGenerator_StatusChanged(object? sender, System.EventArgs e)
        {
            if (SegmentsHost.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                SyncSelectionFromIndex();
                UpdateSegmentVisuals();
            }
        }

        private List<string> GetItemsList()
        {
            if (ItemsSource == null)
                return new List<string>();

            return ItemsSource
                .Cast<object>()
                .Select(item => item?.ToString() ?? string.Empty)
                .ToList();
        }

        private void SegmentPill_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && ItemsSource != null)
            {
                var items = GetItemsList();
                int clickedIndex = items.IndexOf(element.DataContext?.ToString() ?? string.Empty);

                if (clickedIndex >= 0 && clickedIndex != SelectedIndex)
                {
                    SelectedIndex = clickedIndex;
                }
            }
        }

        private void SyncSelectionFromIndex()
        {
            if (ItemsSource == null) return;

            var items = GetItemsList();
            if (items.Count == 0) return;

            _isSyncing = true;
            try
            {
                if (SelectedIndex >= 0 && SelectedIndex < items.Count)
                {
                    SelectedItem = items[SelectedIndex];
                }
            }
            finally
            {
                _isSyncing = false;
            }
        }

        private void SyncSelectionFromItem()
        {
            if (ItemsSource == null || SelectedItem == null) return;

            var items = GetItemsList();
            int index = items.IndexOf(SelectedItem);

            if (index >= 0)
            {
                _isSyncing = true;
                try
                {
                    SelectedIndex = index;
                }
                finally
                {
                    _isSyncing = false;
                }
            }
        }

        private void UpdateSegmentVisuals()
        {
            if (ItemsSource == null) return;

            int count = SegmentsHost.Items.Count;
            var activeBg = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF")!;
            var activeFg = (SolidColorBrush)new BrushConverter().ConvertFromString("#111827")!;
            var inactiveFg = (SolidColorBrush)new BrushConverter().ConvertFromString("#6B7280")!;

            for (int i = 0; i < count; i++)
            {
                if (SegmentsHost.ItemContainerGenerator.ContainerFromIndex(i) is ContentPresenter presenter)
                {
                    presenter.ApplyTemplate();
                    var border = FindVisualChild<Border>(presenter);
                    var textBlock = FindVisualChild<TextBlock>(presenter);

                    if (border != null && textBlock != null)
                    {
                        bool isSelected = i == SelectedIndex;
                        border.Background = isSelected ? activeBg : Brushes.Transparent;
                        textBlock.Foreground = isSelected ? activeFg : inactiveFg;
                        textBlock.FontWeight = isSelected ? FontWeights.SemiBold : FontWeights.Medium;
                    }
                }
            }
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }

                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}