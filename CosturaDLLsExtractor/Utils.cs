using Crayon;
using dnlib.DotNet;
using dnlib.IO;
using System;
using System.IO;
using System.IO.Compression;

namespace CosturaDLLsExtractor
{
    internal static class Utils
    {
        internal static void askFilePath()
        {
            while (!File.Exists(Program.filePath))
            {
                Console.Clear();
                Console.WriteLine("File Path: ");
                Program.filePath = Console.ReadLine().Replace("\"", string.Empty);
                Console.Clear();
            }
        }
        internal static void LoadModule(string filePath)
        {
            try
            {
                Program.Module = ModuleDefMD.Load(filePath);
                Logger.Infos(String.Format("Loaded {0} with success !", Output.Green(Program.Module.Name)));
            }
            catch (Exception ex)
            {
                Logger.Error("This file is not a .Net Assembly !");
                Console.ReadLine();
                Environment.Exit((int)ExitCode.InvalidFile);
            }
        }
        private static byte[] DecompressResource(Stream input)
        {
            MemoryStream output = new MemoryStream();
            new DeflateStream(input, CompressionMode.Decompress).CopyTo(output);

            return output.ToArray();
        }
        internal static void ExtractDLLs(ModuleDefMD mod)
        {
            try
            {
                if (!mod.HasResources)
                {
                    Logger.Error("This module doesn't have any resource !");
                    Console.ReadLine();
                    Environment.Exit((int)ExitCode.NoResources);
                }
                else
                {
                    foreach (Resource resource in mod.Resources)
                    {
                        if (resource.Name.StartsWith("costura.") && resource.Name.EndsWith(".dll.compressed"))
                        {
                            try
                            {
                                EmbeddedResource DLL = mod.Resources.FindEmbeddedResource(resource.Name);
                                DataReader reader = DLL.CreateReader();
                                string name = fixName(resource.Name);
                                using (Stream bufferStream = reader.AsStream())
                                {
                                    string path = Path.Combine(mod.Location.Replace(mod.Name, string.Empty), name);
                                    File.WriteAllBytes(path, DecompressResource(bufferStream));
                                }

                                Logger.Infos(String.Format("Extracted {0} with success !", Output.Green(resource.Name)));

                                Program.ExtractedDLLs++;
                            } catch (Exception ex)
                            {
                                Logger.Error(String.Format("Failed to extract {0} !", Output.Green(resource.Name)));
                                
                                Program.FailedDLLs++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("This file is not a .Net Assembly !");
                Console.ReadLine();
                Environment.Exit((int)ExitCode.InvalidFile);
            }
        }

        internal static string fixName(string name)
        {
            name = name.Replace(".dll.compressed", ".dll");
            if (name == "costura.costura.dll")
                name = name.Replace("costura.costura", "costura");
            else
                name = name.Replace("costura.", string.Empty);
            return name;
        }
    }
}
