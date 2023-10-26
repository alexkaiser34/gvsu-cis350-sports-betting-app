using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.Runtime.Endpoints;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    internal class DbHelper
    {
        private readonly IDynamoDBContext _dbContext;

        public DbHelper(AwsKeys keys) {
            var awsCredentials = new BasicAWSCredentials(keys.accessKey, keys.secretKey);
            var dbCofig = new AmazonDynamoDBConfig() { RegionEndpoint = RegionEndpoint.USEast2 };
            var dbClient = new AmazonDynamoDBClient(awsCredentials, dbCofig);
            _dbContext = new DynamoDBContext(dbClient);
        }

        public async Task<IEnumerable<T>> Get<T>(string id)
        {
            return await _dbContext
                .QueryAsync<T>(id)
                .GetRemainingAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await _dbContext.ScanAsync<T>(conditions).GetRemainingAsync();
            return scan;
        }

        public async Task Post<T>(IEnumerable<T> data)
        {
            var batch = _dbContext.CreateBatchWrite<T>();
            batch.AddPutItems(data);
            await batch.ExecuteAsync();
        }
    }
}
