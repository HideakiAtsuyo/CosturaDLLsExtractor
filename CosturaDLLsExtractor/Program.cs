using Crayon;
using dnlib.DotNet;
using System;
using System.Collections.Generic;

namespace CosturaDLLsExtractor
{
    internal class Program
    {
        public static string filePath = string.Empty;
        public static ModuleDefMD Module = null;
        public static readonly Dictionary<byte[], string> _extractedResources = new Dictionary<byte[], string>();
        public static int ExtractedDLLs = 0, FailedDLLs = 0;

        static void Main(string[] args)
        {
            Console.Title = "Costura DLLs Extractor | github.com/HideakiAtsuyo";
            if(args.Length == 0)
            {
                Utils.askFilePath();
            } else
            {
                filePath = args[0];
                Utils.askFilePath();
            }
            Utils.LoadModule(filePath);
            Utils.ExtractDLLs(Module);
            Logger.Infos(String.Format("Extracted: {0} | Failed to extract: {1}", Output.Green(ExtractedDLLs.ToString()), Output.Red(FailedDLLs.ToString())));
            Console.ReadLine();
        }
    }
}
