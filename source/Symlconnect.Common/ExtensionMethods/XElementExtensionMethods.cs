using System;
using System.Linq;
using System.Xml.Linq;

namespace Symlconnect.Common.ExtensionMethods
{
    public static class XElementExtensionMethods
    {
        public static void ValidateRequiredAttributes(this XElement element, params string[] attributeNames)
        {
            var missingAttributes =
                attributeNames.Where(a => string.IsNullOrWhiteSpace(element.Attribute(a)?.Value)).ToList();
            if (missingAttributes.Any())
            {
                throw new InvalidOperationException(
                    $"Missing required attribute(s) '{string.Join(",", missingAttributes)}' on element: {element}");
            }
        }
    }
}