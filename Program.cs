using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
namespace summSub
{
    class Program
    {
        static void Main(string[] args)
        {

            DirectoryInfo diTop = new DirectoryInfo(@"C:\summSub\summSub\input");
            List<string> strSubnets = new List<string>();
            foreach (var fi in diTop.EnumerateFiles())
            {
                foreach (string line in File.ReadLines(fi.FullName))
                {
                    if (line.Contains("."))
                    {
                        strSubnets.Add(line);
                    }
                }


            }

            int numSubnets = strSubnets.Count;
            IPNetwork[] ipSubnets = new IPNetwork[numSubnets];

            StreamWriter fs = new StreamWriter(@"C:\summSub\summSub\input\test.txt");

            for (int i = 0; i < numSubnets; i++)
            {
                ipSubnets[i] = IPNetwork.Parse(strSubnets[i]);
            }

            IPNetwork[] summIPNetworks = IPNetwork.Supernet(ipSubnets);

            foreach (IPNetwork item in summIPNetworks)
            {
                fs.WriteLine(item.ToString());
                //Console.WriteLine(item);
            }
            Console.WriteLine($"Number of Summarized Subnets: {summIPNetworks.Length}");
            Console.WriteLine($"Raw Number of Subnets: {strSubnets.Count}");
            fs.Close();


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
