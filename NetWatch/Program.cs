using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;


namespace Examples.Net.AddressChanges
{
    public class NetworkingExample
    {
        public static List<string> adapterIPs = new List<string>();
        public static void Main()
        {
            string title = "############ NETWATCH ############ \n\n";



            string options = @"[*] Network adapter IPv4 change event listener!" + "\n\n";
            Console.Write(title);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(options);
            Console.ResetColor();

            Console.WriteLine("[*] Current UP adapters and IPv4 addresses...");
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface n in adapters)
            {
                foreach (UnicastIPAddressInformation ip in n.GetIPProperties().UnicastAddresses)
                {
                    Match match = Regex.Match(ip.Address.ToString(), @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    if (match.Success)
                    {
                        if (n.OperationalStatus == OperationalStatus.Up)
                        {
                            adapterIPs.Add(ip.Address.ToString());
                            Console.WriteLine("[*] Name: " + n.Name + " Description: " + n.Description + " IP: " + ip.Address.ToString());
                        }
                    }
                }
            }

            NetworkChange.NetworkAddressChanged += new
            NetworkAddressChangedEventHandler(AddressChangedCallback);
            
            while (true)
            {
                Console.ReadLine();
            }
        }
        static void AddressChangedCallback(object sender, EventArgs e)
        {

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface n in adapters)
            {
                foreach (UnicastIPAddressInformation ip in n.GetIPProperties().UnicastAddresses)
                {
                    Match match = Regex.Match(ip.Address.ToString(), @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    if (match.Success)
                    {
                        if (!adapterIPs.Contains(ip.Address.ToString()) && n.OperationalStatus == OperationalStatus.Up)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[*] New Network Address Change Event!!!");
                            Console.ResetColor();
                            Console.WriteLine("[*] Name: " + n.Name + " Description: " + n.Description + " IP: " + ip.Address.ToString());
                        }
                        else
                        { }
                    }
                }
            }
        }
    }
}