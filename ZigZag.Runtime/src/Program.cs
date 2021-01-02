using System;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZigZag.Core;
using ZigZag.Core.Commands;
using ZigZag.Core.Serialization;
using ZigZag.Core.Parameters;


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
            var par0 = new IntParameter(loremNode);
            var par1 = new FloatParameter(loremNode);
            var par2 = new FloatParameter(loremNode);


            proj.SubmitCommand(new AddNodeCommand(proj, loremNode));
            proj.SubmitCommand(new AddNodeCommand(loremNode, printNode));
            proj.SubmitCommand(new AddNodeCommand(loremNode, printNode2));
            proj.SubmitCommand(new ConnectParametersCommand(par0, par1));
            proj.SubmitCommand(new ConnectParametersCommand(par0, par2));



            proj.Execute();

            //printNode.Name = "pwintie";
            //printNode2.Name = "Lil pwintie";
            //loremNode.Name = "lorem";
            //loremNode.Parent = printNode;
            //printNode2.Parent = printNode;

            string jsonString;
            var options = new JsonSerializerOptions();
            var objectSerializer = new ZObjectSerializer();
            var objectRefSerializer = new ObjectRefSerializer();
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(objectSerializer);
            options.Converters.Add(objectRefSerializer);
            options.WriteIndented = true;
            
            jsonString = JsonSerializer.Serialize(loremNode, loremNode.GetType(), options);    
            Console.WriteLine(jsonString);

            var n = JsonSerializer.Deserialize(jsonString, typeof(ZObject), options);
            objectRefSerializer.ResolveObjects(objectSerializer.CreatedObjects);


            Console.WriteLine("*");
        }
    }
}
