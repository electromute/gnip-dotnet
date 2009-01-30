using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Gnip.Client.Resource;

namespace Gnip.Client.Utils
{
    public class XmlHelper
    {
        /// <summary>
        /// The singleton XmlSerializer.
        /// </summary>
        private static XmlHelper singleton;

        /// <summary>
        /// Get the Singleton ApplicationUserHelper
        /// </summary>
        public static XmlHelper Instance
        {
            get
            {
                if (XmlHelper.singleton == null)
                {
                    XmlHelper.singleton = new XmlHelper();
                }

                return XmlHelper.singleton;
            }

            set { XmlHelper.singleton = value; }
        }

        /// <summary>
        /// Private due to singleton. Use XmlSerializer.Instance.
        /// </summary>
        protected XmlHelper() { }

        private XmlReaderSettings settings;
        private bool validateXml;

        public bool ValidateXml
        {
            get { return this.validateXml; }
            set
            {
                if (this.validateXml != value)
                {
                    if (value)
                    {
                        if (this.ReaderSettings.Schemas.Count <= 0)
                            this.ReaderSettings.Schemas.Add(GnipSchema.Instance);
                        this.ReaderSettings.ValidationType = ValidationType.Schema;
                    }
                    else
                    {
                        this.ReaderSettings.ValidationType = ValidationType.None;
                    }

                    this.validateXml = value;
                }
            }
        }

        private XmlReaderSettings ReaderSettings
        {
            get
            {
                if (this.settings == null)
                {
                    this.settings = new XmlReaderSettings();
                }

                return this.settings;
            }
        }

        /// <summary>
        /// Unmarshall/Deserialize an object from a stream of XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Deserialize</typeparam>
        /// <param name="stream">The stream to read the XML from.</param>
        /// <returns>A new T</returns>
        public T FromXmlStream<T>(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream, this.ReaderSettings))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Unmarshall/Deserialize an object from a string of XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Serialize</typeparam>
        /// <param name="str">The XML formatted string to deserialize</param>
        /// <returns>A new T</returns>
        public T FromXmlString<T>(string str)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(str), this.ReaderSettings))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Unmarshall/Deserialize an object from a file of XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Deserialize</typeparam>
        /// <param name="path">The file to read the XML from.</param>
        /// <returns>A new T</returns>
        public T FromXmlFile<T>(string path)
        {
            T ret = default(T);

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ret = this.FromXmlStream<T>(fs);
            }

            return ret;
        }

        /// <summary>
        /// Marshall/Serialize an object to a stream as XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Serialize</typeparam>
        /// <param name="value">The object to serialize</param>
        /// <param name="stream">The stream to write the XML to.</param>
        public void ToXmlStream<T>(T value, Stream stream)
        {
            byte[] bytes = ToXmlByteArray<T>(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Marshall/Serialize an object to a string as XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Serialize</typeparam>
        /// <param name="value">The object to serialize</param>
        /// <returns>XML byte array representation of value</returns></pretunrs>
        public byte[] ToXmlByteArray<T>(T value)
        {
            MemoryStream stream = new MemoryStream();
            using (XmlTextWriter writer = new XmlTextWriter(stream, new UTF8Encoding(false)))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, value);
            }

            return stream.ToArray();
        }

        /// <summary>
        /// Marshall/Serialize an object to a string as XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Serialize</typeparam>
        /// <param name="value">The object to serialize</param>
        /// <returns>XML string representation of value</returns></pretunrs>
        public string ToXmlString<T>(T value)
        {
            return StringUtils.ToString(ToXmlByteArray<T>(value));
        }

        /// <summary>
        /// Marshall/Serialize an object to a stream as XML.
        /// </summary>
        /// <typeparam name="T">The type of object to Serialize</typeparam>
        /// <param name="value">The object to serialize</param>
        /// <param name="path">The file to write the XML to.</param>
        public void ToXmlFile<T>(T value, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                this.ToXmlStream<T>(value, fs);
            }
        }
    }
}

