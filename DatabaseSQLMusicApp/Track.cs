using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseMongoMusicApp
{
    internal class Track
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public string VideoURL { get; set; }

        public string Lyrics { get; set; }
    }
}
