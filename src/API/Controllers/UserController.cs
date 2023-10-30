using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IDynamoDBContext _dbContext;

        public UserController(
            IDynamoDBContext dynamoDBContext,
            ILogger<UserController> logger)
        {
            _logger = logger;
            _dbContext = dynamoDBContext;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await _dbContext.ScanAsync<User>(conditions).GetRemainingAsync();
            return scan;
        }

        [HttpPost]
        [Route("Post")]
        public async Task PostUser(UserData inputUser)
        {
            if (inputUser == null)
            {
                return;
            }

            /** Auto increment ID **/
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await _dbContext.ScanAsync<User>(conditions).GetRemainingAsync();
            User newUser = new User()
            {
                UserName = inputUser.UserName,
                Email = inputUser.Email,
                firstName = inputUser.firstName,
                lastName = inputUser.lastName,
                Password = inputUser.Password
            };

            for (int i = 1; i < scan.Count; i++)
            {
                /** assign next available ID to user **/
                if (Int32.Parse(scan[i].id) != i)
                {
                    newUser.id = i.ToString();
                    await _dbContext.SaveAsync<User>(newUser);
                    return;
                }
            }

            /** increment ID **/
            newUser.id = (scan.Count+1).ToString();
            await _dbContext.SaveAsync<User>(newUser);

        }

        [HttpDelete]
        [Route("Delete")]
        public async Task DeleteByID(string id = "123")
        {
            /** For some reason, we can not just delete by the primary key directly
             * We have to load the object by ID, and then store it and delete it **/
            List<User> tmp = await _dbContext.QueryAsync<User>(id).GetRemainingAsync();
            await _dbContext.DeleteAsync<User>(tmp[0]);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<User>> GetByID(string id = "123")
        {
            return await _dbContext
                .QueryAsync<User>(id)
                .GetRemainingAsync();
        }
    }
}