using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ZigZag.Core
{
    public class NodeSerializer : JsonConverter<AbstractNode>
    {
        class UnknownNodeTypeException : Exception
        {
            public UnknownNodeTypeException(string nodeType)
            {
                NodeType = nodeType;
            }

            public string NodeType { get; }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsSubclassOf(typeof(AbstractNode))
                || typeToConvert.Equals(typeof(AbstractNode));
        }


        // requirements of a json node:
        // node must start be its own object
        // node must specify its type as a string first IE
        // "type": "ZigZag.Text.Print"
        // for now the rest is not mandatory

        // create a way to add new fields after name, and before children
        // and make sure works forward and backwards compatible.

        public override AbstractNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonAssert(reader.TokenType == JsonTokenType.StartObject);

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
            JsonAssert(reader.GetString() == "type");

            reader.Read();
            JsonAssert(reader.TokenType == JsonTokenType.String);

            string nodeTypeString = reader.GetString();
            var nodeType = NodeTypeLibrary.GetNodeType(nodeTypeString);

            AbstractNode node;

            if (nodeType is null)
            {
                throw new UnknownNodeTypeException(nodeTypeString);
            }
            else
            {
                node = (AbstractNode)Activator.CreateInstance(nodeType);

                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    reader.Read();
                    JsonAssert(reader.TokenType == JsonTokenType.PropertyName);
                    string propertyName = reader.GetString();

                    switch (propertyName)
                    {
                        case "children":
                            reader.Read();
                            JsonAssert(reader.TokenType == JsonTokenType.StartArray);
                            reader.Read();

                            while (reader.TokenType != JsonTokenType.EndArray)
                            {
                                JsonAssert(reader.TokenType == JsonTokenType.StartObject);

                                var child = JsonSerializer.Deserialize<AbstractNode>(ref reader, options);
                                child.Parent = node;

                                JsonAssert(reader.TokenType == JsonTokenType.EndObject);
                                reader.Read(); // reader.Skip() moved to the end token. Read past it.
                            }
                            JsonAssert(reader.TokenType == JsonTokenType.EndArray);
                            reader.Read(); // Read past EndArray token.
                            break;

                        case "name":
                            reader.Read();
                            JsonAssert(reader.TokenType == JsonTokenType.String);
                            node.Name = reader.GetString();
                            break;

                        /*
                        case "somethingElse":
                            // do other stuff
                            break;
                        */

                        default: // Read and skip.
                            reader.Read();
                            if (reader.TokenType == JsonTokenType.StartArray ||
                                reader.TokenType == JsonTokenType.StartObject)
                            {
                                reader.Skip();
                                reader.Read(); // reader.Skip() moved to the end token. Read past it.
                            }
                            break;
                    }
                }
                JsonAssert(reader.TokenType == JsonTokenType.EndObject);
            }
            return node;
        }

        public override void Write(Utf8JsonWriter writer, AbstractNode node, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteString("type", node.GetType().FullName);

            writer.WritePropertyName("ZigZag.Core.AbstractNode");
            Serialization.WriteAbstractNodePart(node, writer, options);

            switch (node)
            {
                case InputNode inputNode:
                    writer.WritePropertyName("ZigZag.Core.InputNode");
                    Serialization.WriteInputNodePart(inputNode, writer, options);
                    break;
                case OutputNode outputNode:
                    writer.WritePropertyName("ZigZag.Core.OutputNode");
                    Serialization.WriteOutputNodePart(outputNode, writer, options);
                    break;
                case ProcessNode processNode:
                    writer.WritePropertyName("ZigZag.Core.ProcessNode");
                    Serialization.WriteProcessNodePart(processNode, writer, options);
                    break;
            }

            writer.WriteEndObject();
        }

        private void JsonAssert(bool statement)
        {
            if (!statement)
            {
                throw new JsonException();
            }
        }
    }
}
