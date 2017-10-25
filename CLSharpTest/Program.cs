using System;
using System.Linq;
using CorsairLinkSharp.LinkDevice;
using CorsairLinkSharp.LinkDriver.HID;

namespace CLSharpTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            HidDriver h;
            try
            {
                h = HidDriver.Scan().First();
                var x = new Device(h);

                Console.WriteLine(x.Id);
                Console.WriteLine(x.ProductName);
                Console.WriteLine(x.FirmwareId);
                Console.WriteLine(x.Leds.First().ActualColor);
                Console.WriteLine(x.Sensors.First().CurrentTemp);
                //Led l = x.Leds.First();
                //l.Color1 = new Color(0, 255, 255);
                //l.Color2 = new Color(255, 0, 255);
                //l.Color3 = new Color(255, 255, 0);
                //l.Color4 = new Color(255, 255, 255);
                //l.Mode = LedMode.Multi4;

                foreach (var j in x.Fans)
                {
                    Console.WriteLine($"Fan: {j.CurrentRPM}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
