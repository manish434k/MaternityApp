using System.IO.Abstractions;
using System.Xml.Linq;

namespace Symlconnect.Maternity.Wpf.FileSystem
{
    public class XDocumentFileOperations : IXDocumentFileLoad, IXDocumentFileSave
    {
        public XDocument LoadFromFile(FileInfoBase file)
        {
            return XDocument.Load(file.FullName);
        }

        public void SaveToFile(XDocument document, FileInfoBase file)
        {
            document.Save(file.FullName);
        }
    }
}