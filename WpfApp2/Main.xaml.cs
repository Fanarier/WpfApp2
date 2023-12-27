using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace WpfApp2
{
    public partial class Main : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private TotalInterest? priceList;
        private string token;
        public Main(Interest response)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(() => Load());
        }
        public async Task Load()
        {
            List<TotalInterest>? list = await httpClient.GetFromJsonAsync<List<TotalInterest>>("http://localhost:5054/api/PriceList");
            int i = 0;
            Dispatcher.Invoke(() =>
            {
                
                ListPriceList.Items.Clear();
                ListPriceList.ItemsSource = list;
            });

        }
        private async Task Delete()
        {
            using var response = await httpClient.DeleteAsync("http://localhost:5054/api/PriceList/" + priceList?.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellWindow = new SellWindow(token);
            sellWindow.ShowDialog();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow(token!);
            if (productWindow.ShowDialog() == true)
            {
                TotalInterest sell = new TotalInterest
                {
                    Name = productWindow.NameProperty,
                    Price=productWindow.PriceProperty,
                    Id= productWindow.VenorCodeProperty
                };
                JsonContent content = JsonContent.Create(sell);
                using var response = await httpClient.PostAsync("http://localhost:5054/api/PriceList", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            TotalInterest? st = ListPriceList.SelectedItem as TotalInterest;
            ProductWindow productWindow = new ProductWindow(token!, st!);
            if (productWindow.ShowDialog() == true)
            {
                st!.Id = productWindow.VenorCodeProperty;
                st!.Name = productWindow.NameProperty;
                st!.Price = productWindow.PriceProperty;
                JsonContent content = JsonContent.Create(st);
                using var response = await httpClient.PutAsync("http://localhost:5054/api/PriceList", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            TotalInterest? st = ListPriceList.SelectedItem as TotalInterest;
            JsonContent content = JsonContent.Create(st);
            using var response = await httpClient.DeleteAsync("http://localhost:5054/api/PriceList/" + st!.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }

        private void ListPriceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
