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
            if (args.Length != 1 || inputDirectory(args) != true)
            {
                helpRequested();
            }

            DirectoryInfo diTop = new DirectoryInfo(args[0]);
            List<string> strSubnets = new List<string>();

            foreach (FileInfo fi in diTop.EnumerateFiles("*ipranges*"))
            {
                foreach (string line in File.ReadLines(fi.FullName))
                {
                    if (line.Contains(":") || line.Contains("/"))
                    {
                        strSubnets.Add(line);
                    }
                }


            }
            int numSubnets = strSubnets.Count;
            IPNetwork[] ipSubnets = new IPNetwork[numSubnets];

            StreamWriter fs = new StreamWriter(args[0] + "/summarizedSubnets.txt");

            for (int i = 0; i < numSubnets; i++)
            {
                ipSubnets[i] = IPNetwork.Parse(strSubnets[i]);
            }

            IPNetwork[] summIPNetworks = IPNetwork.Supernet(ipSubnets);

            foreach (IPNetwork item in summIPNetworks)
            {
                fs.WriteLine(item.ToString());
            }
            Console.WriteLine();
            Console.WriteLine($"Aggregate Number of Subnets: {strSubnets.Count}");
            Console.WriteLine($"Summarized Subnets: {summIPNetworks.Length}");
            Console.WriteLine();
            Console.WriteLine($"Output file: {args[0] + "\\summarizedSubnets.txt"}");
            Console.WriteLine();
            

            fs.Close();

            void helpRequested()
            {
                Console.WriteLine();
                Console.WriteLine("Summarize Subnets - summSub usage:");
                Console.WriteLine();
                Console.WriteLine("What does summSubb do:  summSub will aggregate all subnets found within any files found within the");
                Console.WriteLine("provided input directory and summarize them.  For example if a file contains:");
                Console.WriteLine();
                Console.WriteLine("192.168.2.0/24");
                Console.WriteLine("192.168.3.0/24");
                Console.WriteLine();
                Console.WriteLine("Then summSub will output a file called 'summarizedSubnets.txt' that contains a single entry:");
                Console.WriteLine();
                Console.WriteLine("192.168.2.0/23");
                Console.WriteLine();
                Console.WriteLine("The new subnet can then be used for both security and routing decisions");
                Console.WriteLine();
                Console.WriteLine("Use of summSub requires the input directory of the files containing the subnets to be summarized");
                Console.WriteLine("supplied as an argument when launching the command as follows:");
                Console.WriteLine();
                Console.WriteLine("-  To parse files in the current working directory, simple use a period:  'summSubb .'");
                Console.WriteLine("-  To parse files in a specific directory, simply supply the directory path: 'summSubb C:\\subnets\\input'");
                Console.WriteLine();
                Console.WriteLine("Notes:  summSubb will parse all files in the input directory with a file name that includes the phrase 'ipranges'");
                Console.WriteLine("and will exclude all lines which do not contain a ':' or a '/'.  summSub should work for both ipv4 and");
                Console.WriteLine("ipv6 summariazation efforts.");
                Console.WriteLine();
                System.Environment.Exit(0);
            }
            bool inputDirectory(string[] inDir)
            {
                if (inDir[0] == ".")
                {
                    args[0] = Environment.CurrentDirectory;
                    return true;
                }
                else
                {
                    inDir[0] = inDir[0].Replace('\\', '/');
                    inDir[0] = inDir[0].Replace("\"", "");
                    int inDirCheck = inDir[0].IndexOf(".");
                    if (inDirCheck != -1)
                    {
                        string dirPath = Path.Combine(Environment.CurrentDirectory, inDir[0]);
                        inDir[0] = (Path.GetFullPath(dirPath));
                    }

                    if (Directory.Exists(inDir[0]) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }

        }
    }
}
