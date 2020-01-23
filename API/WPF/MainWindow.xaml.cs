using API.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Car> Cars { get; set; } = new ObservableCollection<Car>();
        private const string Url= "http://localhost:8080/api";
        //private const string Url = "http://localhost:5000/api";
        public ObservableCollection<Car> AvailableCars { get; set; } = new ObservableCollection<Car>();

        private static readonly HttpClient httpClient = new HttpClient();

        private DateTime SelectedDateValue;
        public DateTime SelectedDate
        {
            get => SelectedDateValue;
            set
            {
                SelectedDateValue = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public Car SelectedCarValue;
        public Car SelectedCar
        {
            get => SelectedCarValue;
            set
            {
                SelectedCarValue = value;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedDate = DateTime.Today;

            FetchAllCars();
            FetchAvailableCars(SelectedDate);
        }

        public async Task FetchAvailableCars(DateTime date)
        {
            var resp = await httpClient.GetAsync(Url + "/cars/available");

            var responseBody = await resp.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<List<Car>>(responseBody);

            AvailableCars.Clear();
            foreach(var car in cars)
            {
                AvailableCars.Add(car);
            }
        }

        public async Task FetchAllCars()
        {
            var resp = await httpClient.GetAsync(Url + "/cars");

            var responseBody = await resp.Content.ReadAsStringAsync();
            var cars = JsonSerializer.Deserialize<IEnumerable<Car>>(responseBody);

            Cars.Clear();
            foreach(var car in cars)
            {
                Cars.Add(car);
            }
            
        }

        private void Grid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Bookings" || e.PropertyName == "DisplayName")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FetchAvailableCars(SelectedDate);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BookCar();
        }

        private async Task BookCar()
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(new Booking { 
                Time = SelectedDate,
                Car = SelectedCar
            }), Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await httpClient.PostAsync(Url + "/Bookings", content);
            resp.EnsureSuccessStatusCode();
            FetchAvailableCars(SelectedDate);
            Close();
        }
    }
}
