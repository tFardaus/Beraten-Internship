using BookShop.Models;
using System.Text.Json;
using System.Xml.Linq;

namespace BookShop.Services.Extensions
{
    public static class SerializationExtensions
    {
        
        public static string ToJson(this Author author)
        {
            return JsonSerializer.Serialize(new
            {
                author.Name,
                author.Biography
            });
        }

        
        public static string ToXml(this Publisher publisher)
        {
            return new XDocument(
                new XElement("Publisher",
                    new XElement("Name", publisher.Name),
                    new XElement("Address", publisher.Address),
                    new XElement("Phone", publisher.Phone)
                )
            ).ToString();
        }

       
        public static (string Name, string Address, string Phone) ParsePublisherXml(this string xml)
        {
            var doc = XDocument.Parse(xml);
            return (
                doc.Root?.Element("Name")?.Value ?? "",
                doc.Root?.Element("Address")?.Value ?? "",
                doc.Root?.Element("Phone")?.Value ?? ""
            );
        }
    }
}
