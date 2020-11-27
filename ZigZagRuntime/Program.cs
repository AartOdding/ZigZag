using NuGet.Repositories;
using NuGet.Packaging;
using NuGet.Frameworks;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace ZigZag.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageLoader.AddLocalRepository("C:/Users/aart_/AppData/Roaming/ZigZag/LocalPackages");
            PackageLoader.LoadPackage("yamldotnet", 8);
            PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0);

            var targetFrameworkAttribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false);

            var reducer = new FrameworkReducer();

            var name = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            var nca21 = NuGetFramework.ParseFolder("netcoreapp2.1");
            var nca31 = NuGetFramework.ParseFrameworkName(name, DefaultFrameworkNameProvider.Instance);
            NuGetFramework[] frameworks = { nca21, nca31 };

            var highest = reducer.ReduceUpwards(frameworks);
            var lowest = reducer.ReduceDownwards(frameworks);


            //new FrameworkNameProvider(DefaultFrameworkNameProvider.Instance)

            var std = "netcoreapp3.1";
            NuGetFramework f = new NuGetFramework(std);

            var r = new NuGetv3LocalRepository("D:/ZigZag/ZigZag/Packages");

            var core = r.FindPackagesById("ZigZag.Core");

            var folderReader = new PackageFolderReader("D:/ZigZag/ZigZag/Packages/ZigZag.Core/0.0.2");
            Console.WriteLine(folderReader.GetIdentity());
            //folder
            foreach (var g in folderReader.GetFrameworkItems())
            {
                Console.WriteLine(g);
            }
            Console.WriteLine("-");
/*            foreach (var g in folderReader.)
            {
                Console.WriteLine(g);
            }*/
            Console.WriteLine("-");
            foreach (var g in folderReader.GetReferenceItems())
            {
                Console.WriteLine(g);
            }

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
                foreach (var g in folderReader.GetLibItems())
                {
                    Console.WriteLine(g);
                }
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
            Console.WriteLine("Hello ", System.IO.Directory.GetCurrentDirectory());
        }
    }
}
