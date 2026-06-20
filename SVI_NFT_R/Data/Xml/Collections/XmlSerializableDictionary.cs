using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ENC.Data.Xml.Collections
{
    /// <summary>
    /// Xml 시리얼라이즈 가능한 딕셔너리 클래스 입니다.
    /// </summary>
    /// <typeparam name="TKey">키 타입</typeparam>
    /// <typeparam name="TValue">벨류 타입</typeparam>
    [Serializable]
    public class XmlSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        private const string ITEM = "item";
        private const string KEY = "key";
        private const string VALUE = "value";

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public XmlSerializableDictionary() : base()
        {
            // default constructor.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializableDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the<see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see  cref= "T:System.Collections.Generic.Dictionary`2" ></see>.</param>
        protected XmlSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// This property is reserved, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"></see> to the class instead.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"></see> that describes the XML representation of the object that is produced by the<see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"></see> method and consumed by the<see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"></see>method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"></see> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement(ITEM);
                reader.ReadStartElement(KEY);
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement(VALUE);
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"></see> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in Keys)
            {
                writer.WriteStartElement(ITEM);
                writer.WriteStartElement(KEY);

                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement(VALUE);
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
    }
}
