// -------------------------------------------------------------------------------------------------------
// LICENSE INFORMATION
//
// - This software is licensed under the MIT shared source license.
// - The "official" source code for this project is maintained at http://oncfext.codeplex.com
//
// Copyright (c) 2010 OpenNETCF Consulting
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
// associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// -------------------------------------------------------------------------------------------------------
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace System
{
    public static partial class StringExtensions
    {
#if !(PCL)
        public static T DeserializeFromXElement<T>(this XElement objectData)
        {
            return (T)objectData.DeserializeFromXElement(typeof(T));
        }

        public static T DeserializeFromXml<T>(this string objectData)
        {
            return (T)DeserializeFromXml(objectData, typeof(T));
        }

        public static object DeserializeFromXml(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (var reader = new NamespaceIgnoringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static object DeserializeFromXElement(this XElement objectData, Type type)
        {
            var serializer = new XmlSerializer(type);

            XElement source;
            var name = type.Name;

            if (objectData.Name == name)
            {
                source = objectData;
            }
            else
            {
                source = objectData.Element(name);
            }

            return source.ToString().DeserializeFromXml(type);
        }
#endif

    }
#if !PCL
    internal class NamespaceIgnoringReader : XmlTextReader
    {
        public NamespaceIgnoringReader(string data)
            : base(new StringReader(data))
        {
        }

        public NamespaceIgnoringReader(TextReader reader)
            : base(reader)
        {
        }

        public override string NamespaceURI
        {
            get { return string.Empty; }
        }
    }

    internal class NamespaceIgnoringWriter : XmlTextWriter
    {
        public NamespaceIgnoringWriter(StringBuilder sb)
            : base(new StringWriter(sb))
        {
        }

        public NamespaceIgnoringWriter(TextWriter writer)
            : base(writer)
        {
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            base.WriteStartElement(prefix, localName, string.Empty);
        }
    }
#endif
}
