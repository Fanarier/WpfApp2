using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private HttpClient client;
        private PriceList admission;
        public ProductWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(()=>LoadAdmissions());
        }
        public ProductWindow(string token, PriceList admission)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadAdmissions());
            VenorCode.Text = Convert.ToString(admission.Id);
            Name.Text = Convert.ToString(admission.Name);
            Price.Text = Convert.ToString(admission.Price);

        }
        private async Task LoadAdmissions()
        {
            List<Product>? list = await client.GetFromJsonAsync<List<Product>>("http://localhost:5054/api/Sell");
        }
        public int VenorCodeProperty
        {
            get { return int.Parse(VenorCode.Text); }
        }
        public string? NameProperty
        {
            get { return Name.Text; }
        }
        public int PriceProperty
        {
            get { return int.Parse(Price.Text); }
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
