using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Events
{
    public class PriceChangedEventArgs : EventArgs
    {
        public readonly decimal lastPrice;
        public readonly decimal newPrice;

        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
        {
            this.lastPrice = lastPrice;
            this.newPrice = newPrice;
        }
    }

    public class Stock
    {
        private string ticker;
        private decimal price;

        public Stock(string ticker)
        {
            this.ticker = ticker;
        }

        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this, e);
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (price == value)
                    return;
                OnPriceChanged(new PriceChangedEventArgs(price, value));
                price = value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var stock = new Stock("WEED") {Price = 27.10M};
            stock.PriceChanged += StockPriceChangedEventHandler;

            stock.Price = 31.59M;
            Console.ReadLine();
        }

        private static async void StockPriceChangedEventHandler(object sender, PriceChangedEventArgs e)
        {
            var heavyCalculationsTask = DoHeavyCalculationsAsync();
            await Task.Delay(1000);
            Console.WriteLine("Writing some text to console while heavy calculations are running in a separate thread.");
            var result = await heavyCalculationsTask;
            Console.WriteLine($"Done, result: {result}.");
            Console.WriteLine("Press <ENTER> to exit.");
        }

        private static Task<int> DoHeavyCalculationsAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Performing heavy calculations.");
                Thread.Sleep(5000);
                Console.WriteLine("Heavy calculations finished.");
                return 1;
            });
        }
    }
}
