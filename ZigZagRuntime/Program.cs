using NuGet.Repositories;
using NuGet.Packaging;
using System;
using System.IO;
using System.Reflection;

namespace ZigZag.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetFrameworkAttribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);
            
            PackageLoader.AddLocalRepository("C:/Users/aart_/AppData/Roaming/ZigZag/LocalPackages");

            PackageLoader.LoadPackage("ZigZag.Core", 0);
            PackageLoader.LoadPackage("yamldotnet", 8);

            //var r = new NuGetv3LocalRepository("D:/ZigZag/ZigZag/Packages");

            /*            var core = r.FindPackagesById("ZigZag.Core");


                        foreach (var v in core)
                        {
                            Console.WriteLine(v.Id);
                            Console.WriteLine(v.ExpandedPath);
                            foreach (var x in v.Files) { Console.WriteLine(x); }
                            var pkgName = v.ExpandedPath + "/" + v.Id + "." + v.Version.ToNormalizedString() + ".nupkg";
                            Console.WriteLine(pkgName);
                            Console.WriteLine(v.Id);
                            Console.WriteLine(v.Id);


                            using FileStream inputStream = new FileStream(pkgName, FileMode.Open);
                            using PackageArchiveReader reader = new PackageArchiveReader(inputStream);
                            NuspecReader nuspec = reader.NuspecReader;
                            //PackageExtractor extractor = new PackageExtractor();

                            Console.WriteLine($"ID: {nuspec.GetId()}");
                            Console.WriteLine($"Version: {nuspec.GetVersion()}");
                            Console.WriteLine($"Description: {nuspec.GetDescription()}");
                            Console.WriteLine($"Authors: {nuspec.GetAuthors()}");

                            Console.WriteLine("Dependencies:");
                            foreach (var dependencyGroup in nuspec.GetDependencyGroups())
                            {
                                Console.WriteLine($" - {dependencyGroup.TargetFramework.GetShortFolderName()}");
                                foreach (var dependency in dependencyGroup.Packages)
                                {
                                    Console.WriteLine($"   > {dependency.Id} {dependency.VersionRange}");
                                }
                            }

                            Console.WriteLine("Files:");
                            foreach (var file in reader.GetFiles())
                            {
                                Console.WriteLine($" - {file}");
                            }

                        }

                        foreach (String a in args)
                        {
                            Console.WriteLine(a);
                        }

                        Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                        Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                        Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                        Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                        Console.WriteLine("Hello ", System.IO.Directory.GetCurrentDirectory());*/
        }
    }
}
