using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DataTransferObjects.Output
{
    [XmlType("Userss")]
    public class UserAndProductOutputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public MiniUserOutputModel[] Users { get; set; }
    }

    [XmlType("User")]
    public class MiniUserOutputModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public MiniSoldProductOutputModel SoldProducts { get; set; }

    }

    [XmlType("SoldProducts")]
    public class MiniSoldProductOutputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public MiniProductNPOutputModel[] Products { get; set; }
    }


    [XmlType("Product")]
    public class MiniProductNPOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
