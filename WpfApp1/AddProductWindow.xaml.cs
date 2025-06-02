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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public string UrunLink { get; private set; }
        public string UrunTuru { get; private set; }
        public int IstenilenFiyat { get; private set; }

        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void BtnKaydet_Click(object sender, RoutedEventArgs e)
        {
            UrunLink = txtLink.Text;

            UrunTuru = txtTur.Text;

            IstenilenFiyat = int.TryParse(txtIstenilenFiyat.Text,out int fiyat) ? fiyat : 0;

            // Burada ekleme işlemi için Onay ver
            this.DialogResult = true;
            this.Close();
        }
    }
}
