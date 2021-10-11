using NNOne.Logic;
using System;

namespace NNOne.MyLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var network = new Network();

            var result = network.UpdateNetwork(0);

            Console.WriteLine(result);
            Console.ReadKey();
        }
        
    }
}
