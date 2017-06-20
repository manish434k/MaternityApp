using System.IO.Abstractions;
using System.Xml.Linq;

namespace Symlconnect.Maternity.Wpf.FileSystem
{
    public interface IXDocumentFileLoad
    {
        XDocument LoadFromFile(FileInfoBase file);
    }
}