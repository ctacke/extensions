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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace System
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool Matches(this string s, string compareTo)
        {
            return string.Compare(s, compareTo) == 0;
        }

        public static bool Matches(this string s, string compareTo, bool ignoreCase)
        {
            if (ignoreCase)
            {
                return string.Compare(s, compareTo, StringComparison.CurrentCultureIgnoreCase) == 0;
            }
            else
            {
                return string.Compare(s, compareTo, StringComparison.CurrentCulture) == 0;
            }
        }

        public static string CropAtLast(this string s, char character)
        {
            var index = s.LastIndexOf(character);
            if (index < 0) return s;

            return s.Substring(0, index);
        }

        public static string SerializeToXml(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder(2048);

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var writer = XmlWriter.Create(sb, settings))
            {
                
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                serializer.Serialize(writer, objectInstance, namespaces);
            }

            return sb.ToString();
        }

        public static XElement SerializeToXElement(this object objectInstance)
        {
            return XElement.Parse(objectInstance.SerializeToXml());
        }

#if!WindowsCE
        public static string ToCamelCase(this string source)
        {
            if (source == null) return null;
            if (source.Trim().Length == 0) return source;

            var words = source.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var first = true;
            var sb = new StringBuilder();
            foreach(var word in words)
            {
                if (first)
                {
                    sb.Append(char.ToLower(word[0]));
                    sb.Append(word.Substring(1));
                    first = false;
                }
                else
                {
                    sb.Append(char.ToUpper(word[0]));
                    sb.Append(word.Substring(1));
                }
            }
            return sb.ToString();
        }
#endif
    }
}
