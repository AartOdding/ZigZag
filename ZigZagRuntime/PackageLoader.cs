using NuGet.Frameworks;
using NuGet.Repositories;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Linq;

namespace ZigZag.Runtime
{
    internal static class PackageLoader
    {

        static private readonly NuGetFramework m_framework;


        static PackageLoader()
        {
            var frameworkName = Assembly.GetExecutingAssembly().
                GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            m_framework = NuGetFramework.ParseFrameworkName(frameworkName, 
                DefaultFrameworkNameProvider.Instance);
        }

        public static void AddLocalRepository(string repositoryPath)
        {
            m_localRepositories.Add(new NuGetv3LocalRepository(repositoryPath));
        }


        private struct Package
        {
            public string name;
            public int versionMajor;
            public LocalPackageInfo localPackage; // null if package not found
        }

        public class PackageLoadException : Exception { }

        // Reasons package loading can fail:
        // a package could not be available
        // conflict with already loaded package
        // package might not have been expanded
        // proper framework might not be available

        // can have conflict inside of a package, package A has Dependencies B and C, 
        // B depends on Xv1 and C depends of Xv2, package should never be loaded

        // can have loops in dependencies, need to take this into account :(


        private static void ResolvePackage(string name, int versionMajor, List<Package> resolvedPackages)
        {

            // First make sure we cant get stuck in an infinite loop by having a circular dependency.
            foreach(Package package in resolvedPackages)
            {
                if (package.name == name && package.versionMajor == versionMajor)
                {
                    // There is a loop, but since this package has already been processed we can return now.
                    return;
                }
            }

            Package p;
            p.name = name;
            p.versionMajor = versionMajor;
            p.localPackage = GetBestPackageVersion(name, versionMajor);
            // When there is no package with the correct major version p.localPackage will be null.

            resolvedPackages.Add(p);

            if (!(p.localPackage is null))
            {
                var nuspec = p.localPackage.Nuspec;

                foreach (var dependencyGroup in nuspec.GetDependencyGroups())
                {
                    foreach (var package in dependencyGroup.Packages)
                    {
                        int dependencyVersionMajor = 0;

                        if (package.VersionRange.HasUpperBound)
                        {
                            dependencyVersionMajor = package.VersionRange.MaxVersion.Major;
                        }
                        else if (package.VersionRange.HasLowerBound)
                        {
                            dependencyVersionMajor = package.VersionRange.MinVersion.Major;
                        }

                        ResolvePackage(package.Id, dependencyVersionMajor, resolvedPackages);
                    }
                }
            } 
        }

        // Returns a list of error messages. If the list is empty, the package was loaded successfully
        public static void LoadPackage(string name, int versionMajor)
        {
            List<Package> packagesToLoad = new List<Package>();
            ResolvePackage(name, versionMajor, packagesToLoad);

            // Check for unavailable packages
            foreach (var p in packagesToLoad)
            {
                if (p.localPackage is null)
                {
                    throw new PackageLoadException();
                }
            }

            // Check for incompatible packages.
            var packageList = new Dictionary<string, Package>(m_loadedPackages);
            
            foreach (var p in packagesToLoad)
            {
                if (packageList.ContainsKey(p.name))
                {
                    // If the major versions are incompatible, throw error. If they are compatible we can just ignore it. 
                    if (packageList[p.name].versionMajor != p.versionMajor)
                    {
                        throw new PackageLoadException();
                    }
                }
                else
                {
                    packageList.Add(p.name, p);
                }
            }

            // If we reach here there are no unavailable packages and no incompatible packages.
            // Load everything that has not yet been loaded. 

            foreach (var p in packagesToLoad)
            {
                GetBestTargetFrameworkMoniker(p);
                //Console.WriteLine(GetBestTargetFrameworkMoniker(p));
                /*                Console.WriteLine(p.localPackage.ExpandedPath);
                                foreach (var fl in p.localPackage.Files)
                                {
                                    Console.WriteLine(fl);
                                }
                                Console.WriteLine(p.name);*/
            }

            NuGet.Frameworks.FrameworkReducer r;

            var x = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;

            Console.WriteLine(x);

            var m = NuGet.Frameworks.DefaultFrameworkMappings.Instance;
            foreach (var sn in m.IdentifierShortNames)
            {
                Console.Write(sn.Key);
                Console.Write("\t");
                Console.WriteLine(sn.Value);
            }
            var fp = new NuGet.Frameworks.DefaultFrameworkNameProvider();
            string z;
            fp.TryGetShortIdentifier(x, out z);
            Console.WriteLine(z);

            NuGet.Frameworks.FrameworkRuntimePair pr;
            
            var y = NuGet.Frameworks.NuGetFramework.ParseFrameworkName(x, fp);
            
            Console.WriteLine(y.DotNetPlatformName);


        }

        private static string GetBestTargetFrameworkMoniker(Package package)
        {
            var frameworkToFolderDict = new Dictionary<NuGetFramework, string>();

            foreach(var dir in Directory.EnumerateDirectories(package.localPackage.ExpandedPath + "/lib"))
            {
                var folder = new DirectoryInfo(dir).Name;
                frameworkToFolderDict.Add(NuGetFramework.ParseFolder(folder), folder);
            }

            FrameworkReducer reducer = new FrameworkReducer();
            var bestFramework = reducer.GetNearest(m_framework, frameworkToFolderDict.Select(kv => kv.Key));

            if (bestFramework is null)
            {
                throw new PackageLoadException();
            }
            return frameworkToFolderDict[bestFramework];
        }

        private static LocalPackageInfo GetBestPackageVersion(string name, int versionMajor)
        {
            LocalPackageInfo bestPackage = null;

            foreach (var repository in m_localRepositories)
            {
                foreach (var package in repository.FindPackagesById(name)
                    .Where(pkg => pkg.Version.Major == versionMajor))
                {
                    if (bestPackage is null || package.Version > bestPackage.Version)
                    {
                        bestPackage = package;
                    }
                }
            }

            return bestPackage;
        }

        private static readonly Dictionary<string, Package> m_loadedPackages = new Dictionary<string, Package>();
        private static readonly List<NuGetv3LocalRepository> m_localRepositories = new List<NuGetv3LocalRepository>();
    }
}
