using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Shell;
using WpfApp1.Models;
using WpfApp1.Services;
using static System.Net.WebRequestMethods;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded; 
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ShowInDataGrid();
        }
        private async Task ShowInDataGrid()
        {
            List<ProductInDB> TempList = JSONDataService.JSONokuma();
            await PriceCheckerService.TumFiyatlariKontrolEtAsync(TempList);

            dgProducts.ItemsSource = null;
            dgProducts.ItemsSource = TempList;

        }

        private async void BtnEkle_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow();
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                string link = addWindow.UrunLink;
                string tur = addWindow.UrunTuru;
                int istenilenFiyat = addWindow.IstenilenFiyat;

                var newProduct = new ProductInDB
                {
                    Link = link,                    
                    Tur = tur,
                    EklenirkenkiFiyat = istenilenFiyat
                };

                List<ProductInDB> tempProduct = new List<ProductInDB>();
                tempProduct.Add(newProduct);

                JSONDataService.JSONguncelleme(tempProduct);

                List<ProductInDB> product = JSONDataService.JSONokuma();
                await ShowInDataGrid();
            }
        }

        private void BtnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            ProductInDB selectedProduct = dgProducts.SelectedItem as ProductInDB;
            
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void BtnSil_Click(object sender, RoutedEventArgs e)
        {
            ProductInDB selectedProduct = dgProducts.SelectedItem as ProductInDB;
            JSONDataService.JSONVeriSilme(selectedProduct);

            List<ProductInDB> product = JSONDataService.JSONokuma();
            await ShowInDataGrid();
        }

        private async void BtnYenile_Click(object sender, RoutedEventArgs e)
        {
            await ShowInDataGrid();
        }
    }
}
