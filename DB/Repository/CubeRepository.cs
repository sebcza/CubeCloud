using System;
using System.Linq;
using System.Threading.Tasks;
using DB.Models;
using DB.Serlializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DB.Repository
{
    public abstract class CubeRepository<TCube> : ICubeRepository<TCube> where TCube : Cube
    {
        protected MongoClient _provider;
        protected IMongoDatabase _db;
        protected string ConnectionString = "mongodb://localhost:27017/hack";
        private IMongoCollection<TCube> _cubes;

        public CubeRepository()
        {
            _provider = new MongoClient(ConnectionString);
            _db = this._provider.GetDatabase(
                MongoUrl.Create(ConnectionString).DatabaseName);
            _cubes = _db.GetCollection<TCube>("cubes");
            Mapping();
        }
        public virtual void Mapping()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            BsonClassMap.RegisterClassMap<TCube>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetSerializer(new GuidSerializer());
                cm.SetIgnoreExtraElements(true);
            });
        }

        public virtual async void CreateCubeAsync(TCube cube)
        {
            await _cubes.InsertOneAsync(cube);
        }

        public virtual async Task<TCube> GetCubeAsync(string address)
        {
            var cube = await _cubes.FindAsync(
                 Builders<TCube>.Filter.Eq("address", address));
            return cube.First<TCube>();
        }

        public virtual async void UpdateCubeAsync(TCube cube)
        {
            await _cubes.FindOneAndReplaceAsync(
                Builders<TCube>.Filter.Eq("_id", cube.Id),
                cube
            );
        }
    }
}