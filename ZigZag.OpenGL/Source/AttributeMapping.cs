using OpenTK.Graphics.OpenGL;


namespace ZigZag.OpenGL
{
    public enum AttributeMapping
    {
        FloatToFloat,

        UInt8ToInt,
        UInt8ToFloat,
        UInt8ToFloatNormalized,

        UInt16ToInt,
        UInt16ToFloat,
        UInt16ToFloatNormalized,

        UInt32ToInt,
        UInt32ToFloat,
        UInt32ToFloatNormalized,

        Int8ToInt,
        Int8ToFloat,
        Int8ToFloatNormalized,

        Int16ToInt,
        Int16ToFloat,
        Int16ToFloatNormalized,

        Int32ToInt,
        Int32ToFloat,
        Int32ToFloatNormalized
    }

    public static class AttributeMappingExtensions
    {
        public static bool IsTargetFloat(this AttributeMapping attributeMapping)
        {
            return !attributeMapping.IsTargetInt();
        }

        public static bool IsTargetInt(this AttributeMapping attributeMapping)
        {
            return attributeMapping == AttributeMapping.UInt8ToInt
                || attributeMapping == AttributeMapping.UInt16ToInt
                || attributeMapping == AttributeMapping.UInt32ToInt
                || attributeMapping == AttributeMapping.Int8ToInt
                || attributeMapping == AttributeMapping.Int16ToInt
                || attributeMapping == AttributeMapping.Int32ToInt;
        }

        public static bool IsNormalized(this AttributeMapping attributeMapping)
        {
            return attributeMapping == AttributeMapping.UInt8ToFloatNormalized
                || attributeMapping == AttributeMapping.UInt16ToFloatNormalized
                || attributeMapping == AttributeMapping.UInt32ToFloatNormalized
                || attributeMapping == AttributeMapping.Int8ToFloatNormalized
                || attributeMapping == AttributeMapping.Int16ToFloatNormalized
                || attributeMapping == AttributeMapping.Int32ToFloatNormalized;
        }

        internal static VertexAttribPointerType GetVertexAttribPointerType(this AttributeMapping attributeMapping)
        {
            return attributeMapping switch
            {
                AttributeMapping.FloatToFloat 
                    => VertexAttribPointerType.Float,

                AttributeMapping.UInt8ToInt or 
                AttributeMapping.UInt8ToFloat or 
                AttributeMapping.UInt8ToFloatNormalized 
                    => VertexAttribPointerType.UnsignedByte,

                AttributeMapping.UInt16ToInt or 
                AttributeMapping.UInt16ToFloat or 
                AttributeMapping.UInt16ToFloatNormalized 
                    => VertexAttribPointerType.UnsignedShort,

                AttributeMapping.UInt32ToInt or 
                AttributeMapping.UInt32ToFloat or 
                AttributeMapping.UInt32ToFloatNormalized 
                    => VertexAttribPointerType.UnsignedInt,

                AttributeMapping.Int8ToInt or 
                AttributeMapping.Int8ToFloat or 
                AttributeMapping.Int8ToFloatNormalized 
                    => VertexAttribPointerType.Byte,

                AttributeMapping.Int16ToInt or 
                AttributeMapping.Int16ToFloat or 
                AttributeMapping.Int16ToFloatNormalized 
                    => VertexAttribPointerType.Short,

                AttributeMapping.Int32ToInt or 
                AttributeMapping.Int32ToFloat or 
                AttributeMapping.Int32ToFloatNormalized 
                    => VertexAttribPointerType.Int,

                _ => throw new System.ArgumentException(),
            };
        }

        internal static VertexAttribIntegerType GetVertexAttribIntegerType(this AttributeMapping attributeMapping)
        {
            return attributeMapping switch
            {
                AttributeMapping.UInt8ToInt or
                AttributeMapping.UInt8ToFloat or
                AttributeMapping.UInt8ToFloatNormalized
                    => VertexAttribIntegerType.UnsignedByte,

                AttributeMapping.UInt16ToInt or
                AttributeMapping.UInt16ToFloat or
                AttributeMapping.UInt16ToFloatNormalized
                    => VertexAttribIntegerType.UnsignedShort,

                AttributeMapping.UInt32ToInt or
                AttributeMapping.UInt32ToFloat or
                AttributeMapping.UInt32ToFloatNormalized
                    => VertexAttribIntegerType.UnsignedInt,

                AttributeMapping.Int8ToInt or
                AttributeMapping.Int8ToFloat or
                AttributeMapping.Int8ToFloatNormalized
                    => VertexAttribIntegerType.Byte,

                AttributeMapping.Int16ToInt or
                AttributeMapping.Int16ToFloat or
                AttributeMapping.Int16ToFloatNormalized
                    => VertexAttribIntegerType.Short,

                AttributeMapping.Int32ToInt or
                AttributeMapping.Int32ToFloat or
                AttributeMapping.Int32ToFloatNormalized
                    => VertexAttribIntegerType.Int,

                _ => throw new System.ArgumentException(),
            };
        }
    }
}
