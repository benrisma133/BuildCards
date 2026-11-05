using FontAwesome.Sharp;
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

namespace BuildCards.Controls.Components
{
    /// <summary>
    /// Interaction logic for InfoCard.xaml
    /// </summary>
    public partial class InfoCard : UserControl
    {
        private bool _isClosed = false;

        public InfoCard()
        {
            InitializeComponent();

            this.Loaded += InfoCard_Loaded;
        }

        // ====================================
        //  Icon Dependency Property
        // ====================================

        public static readonly DependencyProperty InfoIconProperty =
            DependencyProperty.Register(
                    nameof(InfoIcon),
                    typeof(IconChar),
                    typeof(InfoCard),
                    new PropertyMetadata(IconChar.InfoCircle)
                );

        public IconChar InfoIcon
        {
            get { return (IconChar)GetValue(InfoIconProperty); }
            set { SetValue(InfoIconProperty, value); }
        }

        // ====================================
        //  Title Dependency Property
        // ====================================

        public static readonly DependencyProperty InfoTitleProperty =
            DependencyProperty.Register(
                    nameof(InfoTitle),
                    typeof(string),
                    typeof(InfoCard),
                    new PropertyMetadata("Title" ,OnTitleChanged)
                );

        public string InfoTitle
        {
            get { return (string)GetValue(InfoTitleProperty); }
            set { SetValue(InfoTitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = d as InfoCard;
            string? newValue = e.NewValue as string;
        }

        // ====================================
        //  Description Dependency Property
        // ====================================

        public static readonly DependencyProperty InfoDescriptionProperty =
            DependencyProperty.Register(
                    nameof(InfoDescription),
                    typeof(string),
                    typeof(InfoCard),
                    new PropertyMetadata("Description", OnDescriptionChanged)
                );

        public string InfoDescription
        {
            get { return (string)GetValue(InfoDescriptionProperty); }
            set { SetValue(InfoDescriptionProperty, value); }
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = d as InfoCard;
            string? newValue = e.NewValue as string;
        }

        // ======================================
        //  Icon Background Dependency Property
        // ======================================

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register(
                    nameof(IconBackground),
                    typeof(Brush),
                    typeof(InfoCard),
                    new PropertyMetadata(null)
                );

        public Brush IconBackground
        {
            get { return (Brush)GetValue(IconBackgroundProperty); }
            set { SetValue(IconBackgroundProperty, value); }
        }


        public static readonly RoutedEvent CloseClickedEvent = 
            EventManager.RegisterRoutedEvent(
                "CloseClicked", 
                RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler), 
                typeof(InfoCard)
            );

        public event RoutedEventHandler CloseClicked
        {
            add { AddHandler(CloseClickedEvent, value); }
            remove { RemoveHandler(CloseClickedEvent, value); }
        }

        // 2. ملي كيتكليكا زر الإغلاق لداخل (✖️)
        private async void InfoCard_Loaded(object sender, RoutedEventArgs e)
        {
            // كنتسناو 3 ثواني (3000 ميلي ثانية) بلا ما نبلوكيو الشاشة
            await Task.Delay(3000);

            // يلا دازت 3 ثواني واليوزر مازال ما سدهاش بيدو
            if (!_isClosed)
            {
                _isClosed = true;
                // كنطلقو الحدث بحال يلا كليكا على الزر
                RaiseEvent(new RoutedEventArgs(CloseClickedEvent));
            }
        }

        // 4. ملي كيكليكي اليوزر على زر (✖️)
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isClosed)
            {
                _isClosed = true;
                RaiseEvent(new RoutedEventArgs(CloseClickedEvent));
            }
        }

    }
}
