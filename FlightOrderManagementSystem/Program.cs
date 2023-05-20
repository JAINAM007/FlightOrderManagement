using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FlightOrderManagementSystem
{
    class Program
    {
        //create the static flight list
        static List<Flight> LoadFlightSchedule()
        {
            var flights = new List<Flight>
        {
            new Flight { FlightNumber = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 },
            new Flight { FlightNumber = 2, Departure = "YUL", Arrival = "YYC", Day = 1 },
            new Flight { FlightNumber = 3, Departure = "YUL", Arrival = "YVR", Day = 1 },
            new Flight { FlightNumber = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 },
            new Flight { FlightNumber = 5, Departure = "YUL", Arrival = "YYC", Day = 2 },
            new Flight { FlightNumber = 6, Departure = "YUL", Arrival = "YVR", Day = 2 }
        };
            return flights;
        }

        //Render the flights list data
        static void DisplayFlightSchedule(List<Flight> flights)
        {
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight: {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
            }
        }

        //Get the order from json file and add it into list
        static List<Order> LoadOrders(string filePath)
        {
            var orders = new List<Order>();
            var jsonData = File.ReadAllText(filePath);
            var orderData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonData);

            foreach (var orderEntry in orderData)
            {
                var orderNumber = orderEntry.Key;
                var destination = orderEntry.Value["destination"];

                orders.Add(new Order { OrderNumber = orderNumber, Departure = "YUL", Arrival = destination });
            }

            return orders;
        }

        //Update the order list with available flights 
        static void ScheduleOrders(List<Flight> flights, List<Order> orders)
        {
            foreach (var order in orders)
            {
                foreach (var flight in flights)
                {
                    if (flight.LoadedBoxes.Count < 20 && !order.FlightNumber.HasValue && !order.Day.HasValue && flight.Arrival == order.Arrival)
                    {
                        flight.LoadedBoxes.Add(order);
                        order.FlightNumber = flight.FlightNumber;
                        order.Day = flight.Day;
                        break;
                    }
                }
            }
        }

        //Render the user story 2 with order data
        static void DisplayOrderSchedule(List<Order> orders)
        {
            foreach (var order in orders)
            {
                if (!order.FlightNumber.HasValue)
                {
                    Console.WriteLine($"order: {order.OrderNumber}, flightNumber: not scheduled");
                }
                else
                {
                    Console.WriteLine($"order: {order.OrderNumber}, flightNumber: {order.FlightNumber}, departure: {order.Departure}, arrival: {order.Arrival}, day: {order.Day}");
                }
            }
        }
        static void Main(string[] args)
        {
            // Call for US 1
            var flights = LoadFlightSchedule();
            DisplayFlightSchedule(flights);

            // Call for US 2
            var orders = LoadOrders("codingassigmentorders.json");
            ScheduleOrders(flights, orders);
            DisplayOrderSchedule(orders);

            Console.ReadLine();
        }
    }
    #region "Get set parameter"
    public class Flight
    {
        public int FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Day { get; set; }
        public List<Order> LoadedBoxes { get; set; } = new List<Order>();
    }
    public class Order
    {
        public string OrderNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int? FlightNumber { get; set; }
        public int? Day { get; set; }


    }
    #endregion
}
