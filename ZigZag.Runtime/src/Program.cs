using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            //AssemblyResolver assemblyResolver = new AssemblyResolver(AssemblyLoadContext.Default);
            //assemblyResolver.AddAssemblySearchFolder("D:/ZigZag/ZigZag/Dependencies/bin/Release/net5.0");

            //PackageLoader.AddLocalRepository("C:/Users/aart_/AppData/Roaming/ZigZag/LocalPackages");


            bool forceLoadFromDevPath = true;
            List<Type> editors = new List<Type>();

            if (forceLoadFromDevPath)
            {
                var ZigZagRootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../.."));

                TypeLibrary.AddTypes(AssemblyReader.GetAllSubclasses(typeof(ZObject),
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        ZigZagRootDir + "/ZigZag.StandardLibrary/Text/TextData/bin/Debug/net5.0/TextData.dll")));

                TypeLibrary.AddTypes(AssemblyReader.GetAllSubclasses(typeof(ZObject),
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        ZigZagRootDir + "/ZigZag.StandardLibrary/Text/LoremIpsum/bin/Debug/net5.0/LoremIpsum.dll")));

                TypeLibrary.AddTypes(AssemblyReader.GetAllSubclasses(typeof(ZObject),
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        ZigZagRootDir + "/ZigZag.StandardLibrary/Text/Print/bin/Debug/net5.0/Print.dll")));

                editors = AssemblyReader.GetAllSubclasses(typeof(IEditor),
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        ZigZagRootDir + "/ZigZag.Editor/bin/Debug/net5.0/ZigZag.Editor.dll"));
            }
            else
            {
                TypeLibrary.AddTypes(AssemblyReader.GetAllSubclasses(typeof(ZObject), PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0)));
                TypeLibrary.AddTypes(AssemblyReader.GetAllSubclasses(typeof(ZObject), PackageLoader.LoadPackage("ZigZag.Text.Print", 0)));
            }

            IEditor editor = null;
            if (editors.Count > 0)
            {
                editor = (IEditor)Activator.CreateInstance(editors[0]);
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


            editor.OpenEditor();
            editor.ProjectChanged(proj);

            while (editor is not null && !editor.WantsToClose())
            {
                editor.Update();
            }

            Console.WriteLine("*");
        }
    }
}
