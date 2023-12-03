using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DatabaseMongoMusicApp
{
    internal class AlbumsDAO
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Album> _albumCollection;

        public AlbumsDAO(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("musicDatabase");
            _albumCollection = _database.GetCollection<Album>("albums");
        }

        public List<Album> GetAllAlbums()
        {
            return _albumCollection.AsQueryable().ToList();
        }

        public List<Album> SearchTitles(string searchTerm)
        {
            var filter = Builders<Album>.Filter.Regex("AlbumName", new BsonRegularExpression(searchTerm, "i"));
            return _albumCollection.Find(filter).ToList();
        }

        public int AddOneAlbum(Album album)
        {
            _albumCollection.InsertOne(album);
            return 1; // Assuming success
        }

        public List<Track> GetTracksForAlbum(string albumID)
        {
            var filter = Builders<Track>.Filter.Eq("AlbumID", albumID);
            var trackCollection = _database.GetCollection<Track>("tracks");
            return trackCollection.Find(filter).ToList();
        }

        public int AddTrackToAlbum(string albumID, Track track)
        {
            var filter = Builders<Album>.Filter.Eq("_id", albumID);
            var update = Builders<Album>.Update.Push("Tracks", track);

            // Initialize "Tracks" as an empty list if it's null
            var options = new UpdateOptions { IsUpsert = true };
            var arrayFilters = new List<ArrayFilterDefinition>();
            arrayFilters.Add(new BsonDocumentArrayFilterDefinition<Album>(new BsonDocument("i.Tracks", new BsonDocument("$exists", false))));
            options.ArrayFilters = arrayFilters;

            var result = _albumCollection.UpdateOne(filter, update, options);

            return result.ModifiedCount > 0 ? 1 : 0; // Assuming success if at least one document is modified
        }

        public int RemoveTrackFromAlbum(string albumID, string trackID)
        {
            var filter = Builders<Album>.Filter.Eq("_id", albumID);
            var update = Builders<Album>.Update.PullFilter("Tracks", Builders<Track>.Filter.Eq("_id", trackID));

            // Initialize "Tracks" as an empty list if it's null
            var options = new UpdateOptions { IsUpsert = true };
            var arrayFilters = new List<ArrayFilterDefinition>();
            arrayFilters.Add(new BsonDocumentArrayFilterDefinition<Album>(new BsonDocument("i.Tracks", new BsonDocument("$exists", false))));
            options.ArrayFilters = arrayFilters;

            var result = _albumCollection.UpdateOne(filter, update, options);

            return result.ModifiedCount > 0 ? 1 : 0; // Assuming success if at least one document is modified
        }

    }
}

