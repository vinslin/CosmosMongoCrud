using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace CosmosMongoCrud.Models
{
    public class Item
    {
        [BsonId] //primary key ya apply pannum
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("typeofproduct")]
        public int TypeOfProduct { get; set; } 

        [BsonElement("offer")]
        public double Offer { get; set; } 

        [BsonElement("stock")]
        public int Stock { get; set; }

    }
}
