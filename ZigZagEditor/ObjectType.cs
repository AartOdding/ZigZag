using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZagEditor
{
    class ObjectType
    {
        public ObjectType(Type objectType)
        {
            Type = objectType;

            if (objectType.IsSubclassOf(typeof(ZigZag.DataSource)))
            {
                Category = ObjectTypeCategory.DataSource;
            }
            else if (objectType.IsSubclassOf(typeof(ZigZag.Operator)))
            {
                Category = ObjectTypeCategory.Operator;
            }
            else
            {
                Category = ObjectTypeCategory.Object;
            }
        }

        public readonly Type Type;
        public readonly ObjectTypeCategory Category;

    }
}
