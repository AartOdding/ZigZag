using NuGet.Frameworks;
using NuGet.Repositories;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Linq;



// can have conflict inside of a package, package A has Dependencies B and C, 
// B depends on X1.0.0 and C depends of X2.0.0, package should never be loaded

namespace ZigZag.Runtime
{
    internal static class PackageLoader
    {
        public class PackageLoadException : Exception { }

        public static void AddLocalRepository(string repositoryPath)
        {
            m_localRepositories.Add(new NuGetv3LocalRepository(repositoryPath));
        }

        public static void LoadPackage(string name, int versionMajor)
        {
            List<Package> packagesToLoad = new List<Package>();
            CollectDependencies(name, versionMajor, packagesToLoad);

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
            // By calling GetBestFrameworkFolder() later we also ensure that the package has been
            // expanded, and that there is atleast a compatible target framework assembly inside.

            foreach (var p in packagesToLoad)
            {
                // Only load the package if the package is not loaded yet.
                // best load in reverse order, so dependencies go before dependants.
                GetBestFrameworkFolder(p);
            }
        }

        private struct Package
        {
            public string name;
            public int versionMajor;
            public LocalPackageInfo localPackage; // null if package not found
        }

        private static void CollectDependencies(string name, int versionMajor, List<Package> dependencyList)
        {

            // First make sure we cant get stuck in an infinite loop by having a circular dependency.
            foreach (Package package in dependencyList)
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
            p.localPackage = GetHighestCompatiblePackage(name, versionMajor);

            // When there is no package with the correct major version p.localPackage will be null.
            // We still put p in the dependency list, because the fact that the dependency is not 
            // available does not change that it is still a dependency. After all the dependencies
            // have been collected this list can be checked for packages without localPackage, and
            // act accordingly.
            // It is important to note that if a dependency is missing, more indirect dependencies
            // could also be missing.

            dependencyList.Add(p);

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

                        CollectDependencies(package.Id, dependencyVersionMajor, dependencyList);
                    }
                }
            }
        }

        private static string GetBestFrameworkFolder(Package package)
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

        private static LocalPackageInfo GetHighestCompatiblePackage(string name, int versionMajor)
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

        static PackageLoader()
        {
            m_localRepositories = new List<NuGetv3LocalRepository>();
            m_loadedPackages = new Dictionary<string, Package>();
            var frameworkName = Assembly.GetExecutingAssembly().
                GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            m_framework = NuGetFramework.ParseFrameworkName(frameworkName,
                DefaultFrameworkNameProvider.Instance);
        }

        private static readonly NuGetFramework m_framework;
        private static readonly Dictionary<string, Package> m_loadedPackages;
        private static readonly List<NuGetv3LocalRepository> m_localRepositories;
    }
}
