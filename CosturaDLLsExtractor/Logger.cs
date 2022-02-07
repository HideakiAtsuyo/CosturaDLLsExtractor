using Crayon;
using System;

namespace CosturaDLLsExtractor
{
    class Logger
    {
        public static void Infos(string x)
        {
            Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Blue(DateTime.Now.ToString("hh:mm:ss").ToString())}{Output.Yellow("]")}{Output.Yellow("[")}{Output.Blue("INFOS")}{Output.Yellow("]")}: " + "{0}", x));
        }
        public static void Error(string x)
        {
            Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Blue(DateTime.Now.ToString("hh:mm:ss").ToString())}{Output.Yellow("]")}{Output.Yellow("[")}{Output.Red("ERROR")}{Output.Yellow("]")}: " + "{0}", x));
        }
    }
}
