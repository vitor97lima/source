/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.

THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Packaging;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace System.OpenXML
{
	/// <summary>
	/// Stores all of the shared strings while the document is being built.  When building
	/// is done, all the strings can be written to the package at one time.
	/// </summary>
	internal static class SharedStrings
	{
		private static Dictionary<string, int> m_sharedStrings =
			new Dictionary<string, int>();

		/// <summary>
		/// Looks up the index of a string in the shared strings table.
		/// </summary>
		/// <param name="text">The text to look for.</param>
		/// <returns>The index of the string in the shared strings table.</returns>
		public static int LookupString(string text)
		{
			// if the item is in the table, return its index
			if (m_sharedStrings.ContainsKey(text))
				return m_sharedStrings[text];

			// if it's not in the list, add it and return the index
			int newIndex = m_sharedStrings.Count;
			m_sharedStrings.Add(text, newIndex);
			return newIndex;
		}

		/// <summary>
		/// Saves the existing shared strings to the package.
		/// </summary>
		/// <param name="package">The package to save the strings to.</param>
		public static void Save(PackageHelper package)
		{
			// define the uri's for later use
			Uri workbookUri =
				new Uri("/xl/workbook.xml", UriKind.Relative);
			Uri sharedStringsUri =
				new Uri("/xl/sharedStrings.xml", UriKind.Relative);

			// create the new shared strings part
			package.CreateNewPart(sharedStringsUri,
				"application/vnd.openxmlformats-" +
				"officedocument.spreadsheetml.sharedStrings+xml",
				Encoding.UTF8.GetBytes(Resources.SharedStringsTemplate));

			// relate the part to the workbook
			package.CreateInternalRelationship(
				workbookUri, sharedStringsUri,
				"http://schemas.openxmlformats.org/" +
				"officeDocument/2006/relationships/sharedStrings");

			// read the shared strings part into an XmlDocument
			XmlDocument sharedStringsXml =
				package.GetWritablePart(sharedStringsUri);

			// write the new data to the shared strings table
			XPathNavigator sharedStringsNav =
				sharedStringsXml.DocumentElement.CreateNavigator();
			using (XmlWriter writer = sharedStringsNav.AppendChild())
			{
				// write each shared string
				foreach (KeyValuePair<string, int>
					sharedString in m_sharedStrings)
				{
					// write the string item
					writer.WriteStartElement("si", Namespaces.SpreadsheetML);
					writer.WriteElementString("t", Namespaces.SpreadsheetML,
						sharedString.Key);
					writer.WriteEndElement();
				}
			}

			// save the changes to the part
			package.SavePart(sharedStringsUri, sharedStringsXml);
		}

		public static void Clear()
		{
			m_sharedStrings.Clear();
		}
	}
}
