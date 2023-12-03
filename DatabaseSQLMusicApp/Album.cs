using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DatabaseMongoMusicApp
{
    internal class Album
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId ID { get; set; }


        public string AlbumName { get; set; }

        public string ArtistName { get; set; }

        public int Year { get; set; }

        public string ImageURL { get; set; }

        public string Description { get; set; }

        public List<Track> Tracks { get; set; }
    }
}
