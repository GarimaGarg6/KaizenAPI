using KaizenAPI.Models;
using MongoDB.Driver;
using System;
using System.Configuration;

namespace KaizenAPI.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        MongoServer _server;
        public MongoDatabase _database;

        public MongoContext()        //constructor   
        {
            try
            {
                _client = new MongoClient(ConfigurationManager.AppSettings["ConnectionString"]);
                _server = _client.GetServer();
                _database = _server.GetDatabase(ConfigurationManager.AppSettings["MongoDatabaseName"]);
                //var credential = MongoCredential.CreateMongoCRCredential(_database.ToString(), "devNet", "Net#123");
                
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }

        //public IMongoCollection<CMClass> _cMClass
        //{
        //    get
        //    {
        //        return _database.GetCollection<CMClass>("_cMClass");
        //    }

            // Reading credentials from Web.config file   
            //var MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"]; 
            //var MongoUsername = ConfigurationManager.AppSettings["MongoUsername"]; 
            //var MongoPassword = ConfigurationManager.AppSettings["MongoPassword"]; 
            //var MongoPort = ConfigurationManager.AppSettings["MongoPort"];  
            //var MongoHost = ConfigurationManager.AppSettings["MongoHost"];  

            //// Creating credentials  
            //var credential = MongoCredential.CreateMongoCRCredential
            //                (MongoDatabaseName,
            //                 MongoUsername,
            //                 MongoPassword);

            //// Creating MongoClientSettings  
            //var settings = new MongoClientSettings
            //{
            //    Credentials = new[] { credential },
            //    Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            //};
            //_client = new MongoClient(settings);
            //_server = _client.GetServer();
            //_database = _server.GetDatabase(MongoDatabaseName);
        //}
    }
}