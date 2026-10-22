using BuildCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BuildCards.Controls.Cards
{
    /// <summary>
    /// Interaction logic for EnrollmentCard.xaml
    /// </summary>
    public partial class EnrollmentCard : UserControl
    {
        private Enrollment? _currentEnrollment;
        private bool _isDark = true;
        public EnrollmentCard()
        {
            InitializeComponent();
            MainWindow.ThemeChanged += OnThemeChanged;
        }

        public void LoadEnrollment(Enrollment enrollment)
        {
            _currentEnrollment = enrollment;

            // Row 0
            AvatarText.Text = $"{enrollment.StudentFullName[0]}";
            StudentNameText.Text = enrollment.StudentFullName;
            CourseTitleText.Text = enrollment.CourseTitle;

            // Row 2
            ProgressText.Text = enrollment.ProgressText;
            GradeText.Text = enrollment.GradeText;
            EnrollmentDateText.Text = enrollment.EnrollmentDateFormatted;

            // Progress bar — set after layout
            ProgressFill.SizeChanged += (s, e) => UpdateProgressBar();
            Loaded += (s, e) => UpdateProgressBar();

            SetStatus(enrollment.Status);
            UpdateShadow();
        }

        private void UpdateProgressBar()
        {
            if (_currentEnrollment == null) return;
            var parentWidth = ((Grid)ProgressFill.Parent).ActualWidth;
            ProgressFill.Width = parentWidth * ((double)_currentEnrollment.ProgressPercent / 100);
        }

        private void OnThemeChanged(bool isDark)
        {
            _isDark = isDark;
            if (_currentEnrollment != null)
                SetStatus(_currentEnrollment.Status);
            UpdateShadow();
        }

        private void UpdateShadow()
        {
            // rgb(195 155 211)
            if (_isDark)
            {
                CardShadow.Color = Color.FromRgb(195, 155, 211); // accent blue glow
                CardShadow.BlurRadius = 10;
                CardShadow.ShadowDepth = 3;
                CardShadow.Opacity = 0.15;
            }
            else
            {
                CardShadow.Color = Color.FromRgb(195, 155, 211); // black shadow
                CardShadow.BlurRadius = 10;
                CardShadow.ShadowDepth = 3;
                CardShadow.Opacity = 0.15;
            }
        }

        private void SetStatus(string status)
        {
            StatusText.Text = status;

            switch (status?.ToLower())
            {
                case "active":
                    StatusBadge.Background = (Brush)App.Current.Resources["SuccessLightBrush"];
                    StatusText.Foreground = (Brush)App.Current.Resources["SuccessBrush"];
                    break;
                case "completed":
                    StatusBadge.Background = (Brush)App.Current.Resources["InfoLightBrush"];
                    StatusText.Foreground = (Brush)App.Current.Resources["InfoBrush"];
                    break;
                case "cancelled":
                    StatusBadge.Background = (Brush)App.Current.Resources["ErrorLightBrush"];
                    StatusText.Foreground = (Brush)App.Current.Resources["ErrorBrush"];
                    break;
                case "suspended":
                    StatusBadge.Background = (Brush)App.Current.Resources["WarningLightBrush"];
                    StatusText.Foreground = (Brush)App.Current.Resources["WarningBrush"];
                    break;
                default:
                    StatusBadge.Background = (Brush)App.Current.Resources["CardHoverBackgroundBrush"];
                    StatusText.Foreground = (Brush)App.Current.Resources["TextSecondaryBrush"];
                    break;
            }
        }
    }
}
