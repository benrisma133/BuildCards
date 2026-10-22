using System;
using System.Windows;
using System.Windows.Threading;

namespace BuildCards
{
    public partial class TestWindow : Window
    {
        private DispatcherTimer _timer;
        private string[] _texts;
        private int _currentTextIndex;
        private int _currentCharIndex;

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
    }
}