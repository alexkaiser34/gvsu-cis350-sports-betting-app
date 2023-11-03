using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace API.Services
{
    public class DbService<T>
    {
        public IDynamoDBContext dbContext;

        public DbService(IDynamoDBContext dynamoDBContext)
        {
            dbContext = dynamoDBContext;
        }

        public List<ScanCondition> getDateConditions(string key, string beginDate, string endDate = null)
        {

            string end = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            string begin = DateTime.Parse(beginDate).ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            List<ScanCondition> conditions = new List<ScanCondition>();

            if (endDate != null)
            {
                /** return a range of dates **/

                /** Add time to end day to include all games within that day **/
                DateTime tmp = DateTime.Parse(endDate).AddHours(23).AddMinutes(59);
                end = tmp.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                conditions.Add(new ScanCondition(key, ScanOperator.Between, begin, end));
            }
            else
            {
                /** return a single day **/
                conditions.Add(new ScanCondition(key, ScanOperator.BeginsWith, beginDate));
            }

            return conditions;


        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await dbContext.ScanAsync<T>(conditions).GetRemainingAsync();
            return scan;
        }

        public async Task<IEnumerable<T>> GetByDate(string begin, string? end = null)
        {
            IEnumerable<ScanCondition> conditions = getDateConditions("commence_time", begin, end);
            DynamoDBOperationConfig config = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And
            };
            var scan = await dbContext.ScanAsync<T>(conditions, config).GetRemainingAsync();
            return scan;
        }

        public async Task<IEnumerable<T>> GetByID(string id = "123")
        {
            return await dbContext
                .QueryAsync<T>(id)
                .GetRemainingAsync();
        }
    }
}
