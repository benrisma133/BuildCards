using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BuildCards.Controls.Cards
{
    // كلاس التجربة للـ Product
    public class Product
    {
        // المعلومات الرئيسية (للجهة الأمامية)
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }

        // المعلومات الإضافية (للجهة الخلفية)
        public string SKU { get; set; }
        public string Category { get; set; }
        public int StockQuantity { get; set; }
        public string FullSpecifications { get; set; }
    }

    public partial class FlipCard : UserControl
    {
        private bool isFlipped = false;
        public Product CurrentProduct { get; set; }

        public FlipCard()
        {
            InitializeComponent();
            LoadMockProductData();
        }

        // تحميل بيانات تجريبية للمنتج
        private void LoadMockProductData()
        {
            CurrentProduct = new Product
            {
                Name = "Wireless Noise-Canceling Headphones",
                Price = 299.99m,
                ShortDescription = "High-fidelity audio with active noise cancellation and 30-hour battery life.",
                SKU = "WH-1000XM5-BLK",
                Category = "Audio & Electronics",
                StockQuantity = 42,
                FullSpecifications = "Bluetooth 5.2, USB-C Fast Charging, Dual Noise Sensor technology, Multipoint connection, Weight: 250g."
            };

            // ربط البيانات بالواجهات
            // Front
            TxtFrontName.Text = CurrentProduct.Name;
            TxtFrontPrice.Text = $"${CurrentProduct.Price:F2}";
            TxtFrontDescription.Text = CurrentProduct.ShortDescription;

            // Back
            TxtBackSku.Text = CurrentProduct.SKU;
            TxtBackCategory.Text = CurrentProduct.Category;
            TxtBackStock.Text = $"{CurrentProduct.StockQuantity} units";
            TxtBackSpecs.Text = CurrentProduct.FullSpecifications;
        }

        // الحدث المشترك لأزرار More و Back
        private void BtnFlip_Click(object sender, RoutedEventArgs e)
        {
            Flip();
        }

        private void Flip()
        {
            // المرحلة 1: الانكماش إلى منتصف البطاقة (ScaleX = 0)
            DoubleAnimation closeAnim = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            closeAnim.Completed += (s, e) =>
            {
                // تبديل الرؤية عند وصول العرض إلى الصفر
                if (!isFlipped)
                {
                    FrontCard.Visibility = Visibility.Collapsed;
                    BackCard.Visibility = Visibility.Visible;
                }
                else
                {
                    FrontCard.Visibility = Visibility.Visible;
                    BackCard.Visibility = Visibility.Collapsed;
                }

                isFlipped = !isFlipped;

                // المرحلة 2: الفتح المكتمل بالاتجاه المناسب
                DoubleAnimation openAnim = new DoubleAnimation
                {
                    To = isFlipped ? -1 : 1,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
                };

                CardScale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, openAnim);
            };

            CardScale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, closeAnim);
        }
    }
}