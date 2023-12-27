using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public partial class SellWindow : Window
    {
        private HttpClient httpClient;
        private Depositor selectedProduct; 

        public SellWindow(string token)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => Load());
        }

        private async Task Load()
        {
            List<Depositor> list = await httpClient.GetFromJsonAsync<List<Depositor>>("http://localhost:5054/api/Product"); 
            Dispatcher.Invoke(() =>
            {
                ListSell.ItemsSource = null;
                ListSell.Items.Clear();
                ListSell.ItemsSource = list;
            });
        }

        private async Task Save()
        {
            Depositor product = new Depositor 
            {
                Id = Convert.ToInt32(VenorCodeSell.Text),
                DepositName = NameSell.Text,
                InterestRate = Convert.ToInt32(PriceOfSoldSell.Text)
            };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5054/api/Product", product); 
            response.EnsureSuccessStatusCode();
            await Load();
        }

        private async Task Edit()
        {
            if (selectedProduct != null)
            {
                selectedProduct.DepositName = NameSell.Text;
                selectedProduct.InterestRate = Convert.ToInt32(PriceOfSoldSell.Text);

                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"http://localhost:5054/api/Product/{selectedProduct.Id}", selectedProduct); 
                response.EnsureSuccessStatusCode();
                await Load();
            }
        }

        private async Task Delete()
        {
            if (selectedProduct != null)
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:5054/api/Product/{selectedProduct.Id}"); 
                response.EnsureSuccessStatusCode();
                await Load();
            }
        }

        private void ListSell_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = ListSell.SelectedItem as Depositor; 
            if (selectedProduct != null)
            {
                NameSell.Text = selectedProduct.DepositName;
                PriceOfSoldSell.Text = selectedProduct.InterestRate.ToString();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            await Edit();
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            await Delete();
        }
    }
}
