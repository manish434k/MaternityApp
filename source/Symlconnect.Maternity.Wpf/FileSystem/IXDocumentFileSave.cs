using System.IO.Abstractions;
using System.Xml.Linq;

namespace Symlconnect.Maternity.Wpf.FileSystem
{
    public interface IXDocumentFileSave
    {
        void SaveToFile(XDocument document, FileInfoBase file);
    }
}