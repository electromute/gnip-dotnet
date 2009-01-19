using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections;
using NUnit.Framework;
using Gnip.Client.Utils;

namespace Gnip.Client
{
	public abstract class BaseTestCase
	{
        public virtual void SetUp()
        {
            XmlHelper.Instance.ValidateXml = true;
        }

        public virtual void TearDown()
        { }

		protected internal virtual byte[] compress(string data)
		{
			MemoryStream result = new MemoryStream();
            GZipStream compressedStream = new GZipStream(result, CompressionMode.Compress);
            StreamWriter writer = new StreamWriter(compressedStream, Encoding.UTF8);
            writer.Write(data);
            writer.Close();
            compressedStream.Close();
            return result.ToArray();
		}
		
		protected internal virtual string readAll(StreamReader stream)
		{
			StreamReader reader = new StreamReader(stream.BaseStream, stream.CurrentEncoding);
			string fromReader;
			StringBuilder result = new StringBuilder();
			while ((fromReader = reader.ReadLine()) != null)
			{
				result.Append(fromReader);
				result.Append("\n");
			}

            if(result.Length > 0)
                result.Remove(result.Length - 1, 1);

			return result.ToString();
		}
		
		protected internal virtual void assertEquals(MemoryStream expected, MemoryStream actual)
		{
			int fromActual;
			while ((fromActual = actual.ReadByte()) != - 1)
			{
                Assert.AreEqual(expected.ReadByte(), fromActual);
			}

            Assert.AreEqual(expected.ReadByte(), -1);
		}
		
		protected internal virtual string uncompress(byte[] compressedData)
		{
			return readAll(new StreamReader(new GZipStream(new MemoryStream(compressedData), CompressionMode.Decompress)));
		}
		
		protected internal virtual void  assertNotEquals(object object1, object object2)
		{	
            if (object1 == object2 || (object1 != null && object1.Equals(object2)))
            {
                string message = "object " + object1 + " is equal to " + object2;
                Assert.Fail(message);
            }
		}
		
		protected internal virtual void  assertHashNotEqual(object object1, object object2)
		{
			if (object1 == null || object2 == null)
				return ;

			assertNotEquals(object1.GetHashCode(), object2.GetHashCode());
		}
		
		protected internal virtual void  assertHashEqual(object object1, object object2)
		{
			if (object1 == null || object2 == null)
                Assert.Fail("hash code not equal");
            Assert.AreEqual(object1.GetHashCode(), object2.GetHashCode());
		}
		
		protected internal virtual void  assertContains(object expected, ICollection collection)
		{
			string message = "Object: " + expected + " is not in " + collection;

            if ((expected != null && collection == null) || (expected == null && collection != null))
                Assert.Fail(message);
			
			foreach(Object o in collection)
			{
				if (expected.Equals(o))
					return ;
			}
            Assert.Fail(message);
		}
		
		protected internal virtual void  assertDoesNotContain(object obj, ICollection collection)
		{
			if (collection == null)
				return ;

            foreach(object o in collection)
            {
                Assert.AreNotEqual(o, obj, "Object " + obj + " is in collection " + collection);
            }
		}
		
		protected internal virtual void  assertIsA(Type expectedClazz, object instance)
		{
            Assert.AreEqual(expectedClazz, instance.GetType());
		}
		
		protected internal virtual void  assertContains(string expected, string text)
		{
            Assert.IsNotNull(text);
            Assert.IsTrue(text.Contains(expected), "expected string [" + expected + "] not in [" + text + "]");
		}
		
		protected internal virtual void  assertDoesNotContain(string expected, string str)
		{
			string message = "The string [" + expected + "] is in [" + str + "]";
			if (str == null)
                Assert.Fail(message);
            Assert.IsFalse(str.Contains(expected), message);
		}
	}
}