// See https://aka.ms/new-console-template for more information

using Cromatix.MP4Reader;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace MP4Reader
{

    class Program
    {
    
        static string[] files;
        static string Intro = @"GoProMetadataReader
Usage : GoPro files or a folder containing Gopro files can be dropped onto the program file or shortcut.
GoProMetadataReader will attempt to extract coordinate metadata from valid mp4 files, and process it according to the default parameters.
Alternatively files/folder can be dropped onto this window or typed/pasted, then Enter.
File paths should be quoted if they contain spaces, and multiple files separated by spaces.

Warning: Ensure this window is active after dropping files";

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                files = ProcessArgs(args);
                
            }
            else
            {
                files = new string[] { @"D:\Datafiles\WorkRelatedData\Video\CoreBusiness\GH040063.MP4" };
                ProcessFiles();
                //Console.WriteLine(Intro);
                //Console.WriteLine("Tip. Click on this window to activate it after the drop and hit Enter");
                //var input = Console.ReadLine();
                //Console.WriteLine("Enter to continue or any other key to cancel");
                //var key = Console.ReadKey(true);
                ////if (key.Key == ConsoleKey.Escape) return;
                ////files = new string[] { @"D:\Datafiles\WorkRelatedData\Core\Low res videos\GL050039.LRV", @"D:\Datafiles\WorkRelatedData\Core\Low res videos\GL050040.LRV" };
                //if (key.Key == ConsoleKey.Enter)
                //{
                //    try
                //    {
                //        if (!string.IsNullOrEmpty(input))
                //        {
                //            var processedInput = FileArray(input);
                //            files = ProcessArgs(processedInput);
                //            if (files.Length > 0)
                //            {
                //                ProcessFiles();
                //            }
                //            else
                //            {
                //                Console.WriteLine($"No valid input provided, nothing to do!");
                //            }

                //        }
                //        else
                //        {
                //            Console.WriteLine($"No valid input provided, nothing to do!");
                //        }

                //    }
                //    catch (Exception)
                //    {

                //        throw;
                //    }

                //}
                //return;
            }
            Console.WriteLine("Any key to Exit");
            Console.ReadKey();
        }

        static void ProcessFiles()
        {
            var sw = Stopwatch.StartNew();
            //using (var fs = new FileStream(@"D:\Datafiles\WorkRelatedData\Core\Low res videos\GL050039.LRV", FileMode.Open))
                var ct = 0;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var outFile = Path.ChangeExtension(file, "gpx");
                    using (var fs = new FileStream(file, FileMode.Open))
                    {
                        try
                        {
                            MP4MetadataReader reader = new MP4MetadataReader(fs);
                            reader.ProcessGPMFTelemetry(1);
                            //reader.ExportToFile("./out.gpx", ExportFormat.GPX);
                            reader.ExportToFile( outFile, ExportFormat.GPX);
                            ct++;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Unable to process {file}. Not correct GoPro format");
                            //throw;
                        }
                    }

                }
                var plural = ct == 1 ? "" : "s";
                Console.WriteLine($"{ct} GPX file{plural} in {sw.ElapsedMilliseconds}ms. {ct} file{plural} could not be processed");

            }
        }

        static string[] FileArray(string input)
        {
            var arr = FilePathsFromString(input);
            var fls = new List<string>();
            //var fls1
            foreach (var f in arr)
            {
                if (File.Exists(f))
                {
                    fls.Add(f);
                }
                else
                {
                    var arr1 = FilePathsFromString(f);
                    foreach (var f1 in arr1)
                    {
                        fls.Add(f1);
                    }
                }
            }
            return fls.ToArray();
        }

        static string[] FilePathsFromString(string filesToSplit)
        {
            IEnumerable<string> result;
            if (filesToSplit.Contains('"'))
            {
                result = filesToSplit.Split('"').Where(f => !string.IsNullOrWhiteSpace(f));
            }
            else
            {
                result = filesToSplit.Split(' ').Where(f => !string.IsNullOrWhiteSpace(f));
            }

            return result.ToArray();
        }

        static string[] ProcessArgs(string[] fls)
        {
            var fileargs = new List<string>();
            if (fls.Length > 0)
            {
                if (Directory.Exists(fls[0]))
                {
                    return Directory.GetFiles(fls[0], "*.mp4");
                }
                else
                {
                    foreach (var arg in fls)
                    {
                        if (File.Exists(arg))
                        {
                            if (Path.GetExtension(arg).ToLower() == ".mp4")
                            {
                                fileargs.Add(arg);
                            }
                        }
                    }

                }

            }
            return fileargs.ToArray();
        }



    }

}

//string[]filetest = new string[] { @"D:\Datafiles\WorkRelatedData\Video\Video\M043_45-62.MP4", @"D:\Datafiles\WorkRelatedData\Video\Video\M043_24-45.MP4" };
//string[]dirtest = new string[] { @"D:\Datafiles\WorkRelatedData\Video\Video" };
//ProcessArgs(dirtest);



