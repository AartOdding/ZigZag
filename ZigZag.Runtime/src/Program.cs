using System;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZigZag.Core;
using ZigZag.Core.Commands;


namespace ZigZag.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageLoader.AddLocalRepository("C:/Users/aart_/AppData/Roaming/ZigZag/LocalPackages");

            bool forceLoadFromDevPath = true;

            if (forceLoadFromDevPath)
            {
                string devLibPath = "D:/ZigZag/ZigZag/ZigZag.StandardLibrary";
                
                TypeLibrary.AddTypes(AssemblyReader.ReadNodes(
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        devLibPath + "/Text/TextData/bin/Debug/netcoreapp3.1/TextData.dll")));

                TypeLibrary.AddTypes(AssemblyReader.ReadNodes(
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        devLibPath + "/Text/LoremIpsum/bin/Debug/netcoreapp3.1/LoremIpsum.dll")));

                TypeLibrary.AddTypes(AssemblyReader.ReadNodes(
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        devLibPath + "/Text/Print/bin/Debug/netcoreapp3.1/Print.dll")));
            }
            else
            {
                TypeLibrary.AddTypes(AssemblyReader.ReadNodes(PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0)));
                TypeLibrary.AddTypes(AssemblyReader.ReadNodes(PackageLoader.LoadPackage("ZigZag.Text.Print", 0)));
            }

            var proj = new Project();
            // Make sure project can only execute commands on its own children

            var loremNode = TypeLibrary.CreateInstance<Node>("ZigZag.Text.LoremIpsum");
            var printNode = TypeLibrary.CreateInstance<Node>("ZigZag.Text.Print");
            var printNode2 = TypeLibrary.CreateInstance<Node>("ZigZag.Text.Print");

            proj.SubmitCommand(new AddNodeCommand(proj, loremNode));
            proj.SubmitCommand(new AddNodeCommand(loremNode, printNode));
            proj.SubmitCommand(new AddNodeCommand(loremNode, printNode2));


            proj.Execute();

            //printNode.Name = "pwintie";
            //printNode2.Name = "Lil pwintie";
            //loremNode.Name = "lorem";
            //loremNode.Parent = printNode;
            //printNode2.Parent = printNode;

            string jsonString;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new ZObjectSerializer());
            options.WriteIndented = true;
            
            jsonString = JsonSerializer.Serialize(printNode, printNode.GetType(), options);    
            Console.WriteLine(jsonString);

            var n = JsonSerializer.Deserialize(jsonString, typeof(Node), options);

            Console.WriteLine(typeof(Int32).IsAssignableFrom(typeof(Double)));
            Console.WriteLine(typeof(Double).IsAssignableFrom(typeof(Int32)));

            var t = n.GetType();

            while (!(t is null))
            {
                Console.WriteLine(t.FullName);
                t = t.BaseType;
            }
            Console.WriteLine("*");

            foreach (var nodeType in TypeLibrary.Types)
            {
                Console.WriteLine(nodeType.FullName);
            }

            //ZigZag.Text.Print p = new Text.Print();
        }
    }
}
