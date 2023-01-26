using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO;

namespace Flights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string filePath = "c:\\Users\\levig\\Desktop\\legitarsasagok-projekt\\legitarsasagok\\data\\";

        public struct _flight
        {
            public int Id { get; private set; }
            public string Company { get; private set; }
            public string From { get; private set; }
            public string To { get; private set; }
            public int Distance { get; private set; }
            public int Flight_time { get; private set; }
            public int Km_price { get; private set; }
        }

        public class Flight
        {
            public _flight F1;  // transit1
            public _flight F2;  // transit2

            public int Id { get; private set; }
            public string Company { get; private set; }
            public string From { get; private set; }
            public string To { get; private set; }
            public int Distance { get; private set; }
            public int Flight_time { get; private set; }
            public int Km_price { get; private set; }

            public Flight(string sor)
            {
                string[] s = sor.Split(',');
                Id = int.Parse(s[0]);
                Company = s[1];
                From = s[2];
                To = s[3];
                Distance = int.Parse(s[4]);
                Flight_time = int.Parse(s[5]);
                Km_price = int.Parse(s[6]);
            }
        }

        public class City
        {
            public string Name { get; private set; }
            public int Population { get; private set; }

            public City(string sor)
            {
                string[] s = sor.Split(',');
                Name = s[0];
                Population = int.Parse(s[1]);
            }
        }

        int IDX;

        List<City> Cities = new List<City>();
        List<City> CitiesFrom = new List<City>();       // sorted list
        List<City> CitiesTo = new List<City>();         // filtered, sorted

        List<Flight> ALLFlights = new List<Flight>();    
        List<Flight> Flights = new List<Flight>();      // filtered flights

        public string flightFrom => Flights[IDX].From;
        public string flightTo => Flights[IDX].To;
        public string flightCompany => Flights[IDX].Company;
        public string flightFlightTime
        {
            get { return $"{ Flights[IDX].Flight_time / 60}:{ Flights[IDX].Flight_time % 60 }"; }
        }
        public string results => $"{IDX + 1}/{Flights.Count}";

        private int _price = 0;
        public string price => $"{_price}$";

        public string flightFrom1 => Flights[IDX].F1.From;
        public string flightTo1 => Flights[IDX].F1.To;
        public string flightCompany1 => Flights[IDX].F1.Company;
        public string flightFlightTime1
        {
            get { return $"{ Flights[IDX].F1.Flight_time / 60}:{ Flights[IDX].F1.Flight_time % 60 }"; }
        }

        public string flightFrom2 => Flights[IDX].F2.From;
        public string flightTo2 => Flights[IDX].F2.To;
        public string flightCompany2 => Flights[IDX].F2.Company;
        public string flightFlightTime2
        {
            get { return $"{ Flights[IDX].F2.Flight_time / 60}:{ Flights[IDX].F2.Flight_time % 60 }"; }
        }
        
        public MainWindow()
        {
            foreach (string sor in File.ReadAllLines(filePath + "flight.txt").Skip(1))
            {
                ALLFlights.Add(new Flight(sor));
            }
            foreach (var item in ALLFlights)
            {
                Flights.Add(item);
            }
            foreach (string sor in File.ReadAllLines(filePath + "city.txt").Skip(1))
            {
                Cities.Add(new City(sor));
            }
            foreach (var item in Cities.OrderBy(x => x.Name))
            {
                CitiesFrom.Add(item);
            }

            DataContext = this;
            InitializeComponent();
            foreach (var item in CitiesFrom)
            {
                cmbFrom.Items.Add(item.Name);
            }
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            IDX = IDX >= Flights.Count - 1 ? Flights.Count - 1 : IDX + 1;
            // to refresh ALL bindings
            DataContext = null;
            DataContext = this;
        }

        private void btnBackward_Click(object sender, RoutedEventArgs e)
        {
            IDX = IDX > 0 ? IDX - 1 : 0;
            // to refresh ALL bindings
            DataContext = null;
            DataContext = this;
        }

        private void cmbFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFrom.SelectedItem.ToString() == "")
                return;
            cmbTo.Items.Clear();
            var tmp = Flights.FindAll(x => x.From == cmbFrom.SelectedItem.ToString()).GroupBy(x => x.To);
            foreach (var item in tmp)
            {
                cmbTo.Items.Add(item.Key);
            }
        }

        private void cmbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            int passangers, kids = 0;
                
            int.TryParse(txtPassangers.Text, out passangers);
            int.TryParse(txtPassangers.Text, out passangers);
            if (passangers > 0 || passangers < 999999999) {
                _price = int.Parse(txtPassangers.Text) * 500;
            }
            else
            {
                MessageBox.Show("Hibás érték");
            }
            DataContext = null;
            DataContext = this;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFrom.SelectedItem != null && cmbTo.SelectedItem != null)
            {
                Flights.Clear();
                var tmp = ALLFlights.FindAll(x => x.From == cmbFrom.SelectedItem.ToString() && x.To == cmbTo.SelectedItem.ToString());
                foreach (var item in tmp)
                {
                    Flights.Add(item);
                }

                IDX = 0;
                DataContext = null;
                DataContext = this;
            }
        }
    }
}
