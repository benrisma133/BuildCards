using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BuildCards
{
    public partial class TestWindow : Window
    {
        private DispatcherTimer _timer;
        private string[] _texts;
        private int _currentTextIndex;
        private int _currentCharIndex;

        private double _targetVerticalOffset = 0;
        private double _currentVerticalOffset = 0;
        private bool _isScrolling = false;

        // 0.15 gives a nice browser-like fluidity
        private readonly double _scrollSmoothness = 0.15;

        public TestWindow()
        {
            InitializeComponent();

            _texts = new string[]
            {
                "Welcome to BuildCards!",
                "This is the second message.",
                "And this is the third one!"
            };

            _currentTextIndex = 0;
            StartTypewriterEffect();

            SetupSmoothScrolling();
        }

        private void StartTypewriterEffect()
        {
            _currentCharIndex = 0;
            TypewriterText.Text = "";

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(30);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string currentText = _texts[_currentTextIndex];

            if (_currentCharIndex < currentText.Length)
            {
                TypewriterText.Text += currentText[_currentCharIndex];
                _currentCharIndex++;
            }
            else
            {
                // Finished typing current text, pause then move to next
                _timer.Stop();
                _timer.Tick -= Timer_Tick;

                var pauseTimer = new DispatcherTimer();
                pauseTimer.Interval = TimeSpan.FromMilliseconds(1000);
                pauseTimer.Tick += (s, args) =>
                {
                    pauseTimer.Stop();

                    // Move to next text, loop back to 0 if at end
                    _currentTextIndex = (_currentTextIndex + 1) % _texts.Length;
                    StartTypewriterEffect();
                };
                pauseTimer.Start();
            }
        }

        private void SetupSmoothScrolling()
        {
            // Apply all XAML properties directly in C#
            MainScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            MainScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;

            // CanContentScroll must be false for pixel-based smooth scrolling to work
            MainScrollViewer.CanContentScroll = false;
            MainScrollViewer.PanningMode = PanningMode.VerticalOnly;

            // Wire up the mouse wheel event
            MainScrollViewer.PreviewMouseWheel += MainScrollViewer_PreviewMouseWheel;
        }

        private void MainScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            // Update target offset based on wheel rotation
            _targetVerticalOffset -= (e.Delta * 1.2);

            // Clamp target to min and max boundaries
            if (_targetVerticalOffset < 0)
                _targetVerticalOffset = 0;
            else if (_targetVerticalOffset > MainScrollViewer.ScrollableHeight)
                _targetVerticalOffset = MainScrollViewer.ScrollableHeight;

            // Hook into the rendering loop if not already scrolling
            if (!_isScrolling)
            {
                _isScrolling = true;
                CompositionTarget.Rendering += SmoothScroll;
            }
        }

        private void SmoothScroll(object sender, EventArgs e)
        {
            // Move current offset a percentage closer to the target offset
            _currentVerticalOffset += (_targetVerticalOffset - _currentVerticalOffset) * _scrollSmoothness;

            MainScrollViewer.ScrollToVerticalOffset(_currentVerticalOffset);

            // Stop rendering loop when we are visually at the target
            if (Math.Abs(_targetVerticalOffset - _currentVerticalOffset) < 0.5)
            {
                _isScrolling = false;
                _currentVerticalOffset = _targetVerticalOffset; // Snap to absolute final pixel
                MainScrollViewer.ScrollToVerticalOffset(_currentVerticalOffset);

                CompositionTarget.Rendering -= SmoothScroll;
            }
        }
    }
}