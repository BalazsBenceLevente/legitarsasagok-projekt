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
    public partial class MainWindow : Window
    {

        string filePath = "c:\\Users\\levig\\Desktop\\legitarsasagok-projekt\\legitarsasagok\\data\\";

        public struct _flight
        {
            public int Id { get; set; }
            public string Company { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public int Distance { get; set; }
            public int Flight_time { get; set; }
            public int Km_price { get; set; }
        }

        public class Flight
        {
            public _flight F;   // direct
            public _flight F1;  // transit1
            public _flight F2;  // transit2

            public Flight(string sor)
            {
                string[] s = sor.Split(',');
                F.Id = int.Parse(s[0]);
                F.Company = s[1];
                F.From = s[2];
                F.To = s[3];
                F.Distance = int.Parse(s[4]);
                F.Flight_time = int.Parse(s[5]);
                F.Km_price = int.Parse(s[6]);
            }
            public Flight(_flight f, _flight f1, _flight f2)
            {
                F = f;
                F1 = f1;
                F2 = f2;
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

        public string flightFrom => (Flights.Count > 0) ? Flights[IDX].F.From : "";
        public string flightTo => (Flights.Count > 0) ? Flights[IDX].F.To : "";
        public string flightCompany => (Flights.Count > 0) ? Flights[IDX].F.Company : "";
        public string flightFlightTime
        {
            get
            {
                if (Flights.Count == 0) return "0:0";
                else return $"{ Flights[IDX].F.Flight_time / 60}:{ Flights[IDX].F.Flight_time % 60 }";
            }
        }
        public string results
        {
            get
            {
                if (Flights.Count > 0) return $"{IDX + 1}/{Flights.Count}";
                else return "-";
            }
        }

        private int _price = 0;
        public string price => $"{_price} Ft";

        public string flightFrom1 => (Flights.Count > 0) ? Flights[IDX].F1.From : "";
        public string flightTo1 => (Flights.Count > 0) ? Flights[IDX].F1.To : "";
        public string flightCompany1 => (Flights.Count > 0) ? Flights[IDX].F1.Company : "";
        public string flightFlightTime1
        {
            get
            {
                if (Flights.Count == 0) return "0:0";
                else return $"{ Flights[IDX].F1.Flight_time / 60}:{ Flights[IDX].F1.Flight_time % 60 }";
            }
        }

        public string flightFrom2 => (Flights.Count > 0) ? Flights[IDX].F2.From : "";
        public string flightTo2 => (Flights.Count > 0) ? Flights[IDX].F2.To : "";
        public string flightCompany2 => (Flights.Count > 0) ? Flights[IDX].F2.Company : "";
        public string flightFlightTime2
        {
            get
            {
                if (Flights.Count == 0) return "0:0";
                else return $"{ Flights[IDX].F2.Flight_time / 60}:{ Flights[IDX].F2.Flight_time % 60 }";
            }
        }

        public MainWindow()
        {
            foreach (string sor in File.ReadAllLines(filePath + "flight.txt").Skip(1))
            {
                ALLFlights.Add(new Flight(sor));
            }
            //foreach (var item in ALLFlights)
            //{
            //    Flights.Add(item);
            //}
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

        void _fillcmbTo()
        {
            if (cmbFrom.SelectedItem == null)
                return;
            if (chkTransit.IsChecked == false)  // direct flights only
            {
                cmbTo.Items.Clear();
                var tmp = ALLFlights.FindAll(x => x.F.From == cmbFrom.SelectedItem.ToString()).GroupBy(x => x.F.To);
                foreach (var item in tmp)
                {
                    cmbTo.Items.Add(item.Key);
                }
            }
            else  // transit
            {
                cmbTo.Items.Clear();
                foreach (var item in Cities.OrderBy(x => x.Name))
                {
                    cmbTo.Items.Add(item.Name);
                }
            }
        }

        private void cmbFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _fillcmbTo();
        }

        private void chkTransit_Click(object sender, RoutedEventArgs e)
        {
            _fillcmbTo();
            Flights.Clear();
            IDX = 0;
            DataContext = null;
            DataContext = this;
        }

        private void cmbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Flights.Clear();
            IDX = 0;
            DataContext = null;
            DataContext = this;
        }

        int _calc(int distance, int km_price, string to, int passengers, int kids)
        {
            if (to == null) return 0;
            double one = ((distance * 1.1) * km_price) * 1.27;
            int population = Cities.FirstOrDefault(x => x.Name == to).Population;
            if (population < 2000000)
            {
                one *= 1.05;
            }
            else if (population < 10000000)
            {
                one *= 1.075;
            }
            else
            {
                one *= 1.1;
            }

            double sum = one * passengers * ((passengers > 10) ? 0.9 : 1.0);
            sum -= kids * one * 0.2;

            return (int)sum;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            int passengers, kids = 0;

            int.TryParse(txtPassangers.Text, out passengers);
            int.TryParse(txtKids.Text, out kids);
            _price = _calc(Flights[IDX].F.Distance, Flights[IDX].F.Km_price, Flights[IDX].F.To, passengers, kids);
            // transits
            _price += _calc(Flights[IDX].F1.Distance, Flights[IDX].F1.Km_price, Flights[IDX].F1.To, passengers, kids);
            _price += _calc(Flights[IDX].F2.Distance, Flights[IDX].F2.Km_price, Flights[IDX].F2.To, passengers, kids);
            DataContext = null;
            DataContext = this;
        }

        void _search(string from, string to)
        {
            List<Flight> tmp = ALLFlights.FindAll(x => x.F.To == to && x.F.From != from);   // without direct flights

            foreach (var item in tmp)
            {
                foreach (var item1 in ALLFlights.FindAll(x => x.F.To == item.F.From && x.F.From == from))
                {
                    Flights.Add(new Flight(item1.F, item.F, item1.F2));     // one transit
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFrom.SelectedItem != null && cmbTo.SelectedItem != null)
            {
                Flights.Clear();

                if (chkTransit.IsChecked == true)   // transit
                {
                    _search(cmbFrom.SelectedItem.ToString(), cmbTo.SelectedItem.ToString());
                }
                else
                {
                    var tmp = ALLFlights.FindAll(x => x.F.From == cmbFrom.SelectedItem.ToString() && x.F.To == cmbTo.SelectedItem.ToString());
                    foreach (var item in tmp)
                    {
                        Flights.Add(item);
                    }
                }

                IDX = 0;
                DataContext = null;
                DataContext = this;
            }
        }
    }
}
