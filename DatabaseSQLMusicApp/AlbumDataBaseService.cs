using DatabaseMongoMusicApp;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseSQLMusicApp
{
    internal class AlbumDataBaseService
    {
        private const string CONNECTION = "mongodb+srv://myAtlasDBUser:myatlas-001@myatlasclusteredu.gao8wnm.mongodb.net/?retryWrites=true&w=majority";

        private const string DATABASENAME = "musicDatabase";
        private const string ALBUMCOLLECTION = "albums";

        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Album> albumCollection;

        public AlbumDataBaseService()
        {
            client = new MongoClient(CONNECTION);
            database = client.GetDatabase(DATABASENAME);
            albumCollection = database.GetCollection<Album>(ALBUMCOLLECTION);
        }

        public List<Album> getAll()
        {
            var results = albumCollection.Find(x => true).ToList();

            return results;
        }


        public Album getOne(string id)
        {
            Album result = albumCollection.Find(x => x.ID == ObjectId.Parse(id)).First();

            return result;
        }

        public void addOne(Album album)
        {
            albumCollection.InsertOne(album);
        }

        internal List<Album> search(string searchTerm)
        {
            var results = albumCollection.Find(x => x.AlbumName.ToLower().Contains(searchTerm.ToLower())).ToList();
            return results;
        }

        internal int deleteOne(string itemNumber)
        {
            long results = albumCollection.DeleteOne(x => x.ID.ToString() == itemNumber).DeletedCount;

            return (int)results;
        }




        internal Album addTrackToAlbum(Album albumWhoGetsNewTrack, Track newTrack)
        {
            // Check if the Tracks property is null, and initialize it if needed
            if (albumWhoGetsNewTrack.Tracks == null)
            {
                albumWhoGetsNewTrack.Tracks = new List<Track>();
            }

            albumWhoGetsNewTrack.Tracks.Add(newTrack);

            albumCollection.FindOneAndReplace(a => a.ID == albumWhoGetsNewTrack.ID, albumWhoGetsNewTrack);

            return albumWhoGetsNewTrack;
        }


        internal bool deleteOneTrack(Album selectedAlbum, string trackIdNumber) 
        { 

        Track foundTrack = selectedAlbum.Tracks.Find(x => x.ID == trackIdNumber);
        
        bool result = selectedAlbum.Tracks.Remove(foundTrack);
        
        albumCollection.FindOneAndReplace(a=> a.ID == selectedAlbum.ID, selectedAlbum) ;
           
        return result;
        }

}
}
