using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;


namespace ZigZagEditor
{
    internal class Plugin
    {
        public Plugin(Assembly _assembly)
        {
            assembly = _assembly;
            objectTypeMap = new Dictionary<ulong, ObjectType>();

            foreach (var t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(ZigZag.Object)))
                {
                    ulong id = Identifier.Create();
                    ObjectType objectType = new ObjectType(t);
                    objectTypeMap.Add(id, objectType);
                    ObjectTypeMap.Insert(id, objectType);
                    NativeInterop.ObjectInterop.onNewObjectTypeAdded(t.FullName, id, objectType.Category);
                }
            }
        }

        public readonly Assembly assembly;
        public readonly Dictionary<ulong, ObjectType> objectTypeMap;

    }
}
