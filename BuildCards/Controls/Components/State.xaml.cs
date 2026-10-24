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
    /// Interaction logic for State.xaml
    /// </summary>
    public partial class State : UserControl
    {
        public State()
        {
            InitializeComponent();
        }

        // ===================================
        //  Card Title Dependency Property
        // ===================================

        public static readonly DependencyProperty CardTitleProperty =
            DependencyProperty.Register(
                    nameof(CardTitle),
                    typeof(string),
                    typeof(State),
                    new PropertyMetadata("Card Title" , OnTitleChanged)
                );

        public string CardTitle
        {
            get {  return (string)GetValue(CardTitleProperty);}
            set { SetValue(CardTitleProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = d as State;
            string? newTitle = e.NewValue as string;
        }

        // ===================================
        //  Card Value Dependency Property
        // ===================================

        public static readonly DependencyProperty CardValueProperty =
            DependencyProperty.Register(
                    nameof(CardValue),
                    typeof(string),
                    typeof(State),
                    new PropertyMetadata("Card Value", OnValueChanged)
                );

        public string CardValue
        {
            get { return (string)GetValue(CardValueProperty); }
            set { SetValue(CardValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = d as State;
            string? newValue = e.NewValue as string;
        }

    }
}
