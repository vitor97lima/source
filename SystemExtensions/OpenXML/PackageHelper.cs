/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.

THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Packaging;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace System.OpenXML
{
    /// <summary>
    /// This class encapsulates the common functions performed
    /// when working with a package located in memory.
    /// </summary>
    internal sealed class PackageHelper : IDisposable
    {
        #region Private Fields
        private MemoryStream m_packageData;
		private Package m_package;

		public Package Package
		{
			get { return m_package; }
			set { m_package = value; }
		}
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of a package basedc on the data provided.
        /// </summary>
        /// <param name="data">Byte array representing the package.</param>
        public PackageHelper(byte[] data)
        {
            // load the memory stream
            m_packageData = new MemoryStream();
            m_packageData.Write(data, 0, data.Length);

            // open the package
            m_package =
                Package.Open(m_packageData,
                    FileMode.Open,
                    FileAccess.ReadWrite); // use read write access
        }
        #endregion

        #region Disposal Methods
        /// <summary>
        /// Close the package and the memory stream
        /// </summary>
        public void Dispose()
        {
            m_package.Close();
            m_packageData.Dispose();
        }
        #endregion

        #region Package Management Methods
        /// <summary>
        /// Creates a relationship from one package part to another.
        /// </summary>
        /// <param name="sourceUri">The uri of the source part.</param>
        /// <param name="targetUri">The relative path representing the location of the target part based on the source part.</param>
        /// <param name="relationshipType">The type of relationship to create.</param>
        /// <returns>The ID of the new relationship.</returns>
        public string CreateInternalRelationship(Uri sourceUri, Uri targetUri, string relationshipType)
        {
            // open the source part
            PackagePart sourcePart = m_package.GetPart(sourceUri);

            // create the relationship
            PackageRelationship relationship =
                sourcePart.CreateRelationship(
                    PackUriHelper.GetRelativeUri(sourceUri, targetUri),
                    TargetMode.Internal, relationshipType);

            // return the new rel id
            return relationship.Id;
        }

        /// <summary>
        /// Saves the package data stored in the internal memory stream to a file.
        /// </summary>
        /// <param name="filename">The filename to write the package to</param>
        public void Save(string filename)
        {
            // flush and data in the package buffers to the stream
            m_package.Flush();
            m_package.Close();

            // write the stream to the output file
            using (FileStream outputStream = File.Create(filename))
                m_packageData.WriteTo(outputStream);

            // close the stream
            m_packageData.Close();
        }
        #endregion

        #region Part Management Methods
        /// <summary>
        /// Creates a new part containing the data provided.
        /// </summary>
        /// <param name="partUri">The uri of the part to create.</param>
        /// <param name="contentType">The content type for the new part.</param>
        /// <param name="data">The data to initially load into the new part.</param>
        /// <returns></returns>
        public PackagePart CreateNewPart(Uri partUri, string contentType, byte[] data)
        {
            // create the part
            PackagePart newPart =
                m_package.CreatePart(partUri, contentType);

            // write the data into the part
            using (Stream partStream =
                newPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                partStream.Write(data, 0, data.Length);
            }

            // return the new package part
            return newPart;
        }

        /// <summary>
        /// Opens the part and loads the XML into an XPathDocument.
        /// </summary>
        /// <param name="partUri">The uri of the part to open.</param>
        /// <returns>Read only XPathDocument containing the xml from the part.</returns>
        public XPathDocument GetReadOnlyPart(Uri partUri)
        {
            // retrieve the part
            PackagePart readOnlyPart = m_package.GetPart(partUri);

            // load the part into a XPathDocument
            using (Stream partStream = readOnlyPart.GetStream(FileMode.Open, FileAccess.Read))
                return new XPathDocument(partStream);
        }

        /// <summary>
        /// Opens the part and loads the XML into an XmlDocument.
        /// </summary>
        /// <param name="partUri">The uri of the part to open.</param>
        /// <returns>XmlDocument containing the xml from the part.</returns>
        public XmlDocument GetWritablePart(Uri partUri)
        {
            // get the part
            PackagePart writablePart = m_package.GetPart(partUri);

            // load the part into a XmlDocument
            XmlDocument partXml = new XmlDocument();
            using (Stream partStream = writablePart.GetStream(FileMode.Open, FileAccess.Read))
                partXml.Load(partStream);

            // return the document
            return partXml;
        }

        /// <summary>
        /// Replaces all content in the part with the XML in the XmlDocument.
        /// </summary>
        /// <param name="partUri">The uri of the part to replace.</param>
        /// <param name="partXml">XmlDocument containing the xml to place into the part.</param>
        public void SavePart(Uri partUri, XmlDocument partXml)
        {
            // get the part
            PackagePart writablePart = m_package.GetPart(partUri);

            // load the part into a XmlDocument
            using (Stream partStream =
                writablePart.GetStream(FileMode.Open, FileAccess.Write))
            {
                partStream.SetLength(0);
                partXml.Save(partStream);
            }
        }
        #endregion
    }
}
