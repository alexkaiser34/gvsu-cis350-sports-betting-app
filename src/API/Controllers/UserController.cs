using Amazon.DynamoDBv2.DataModel;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DbService<User> _dbService;


        public UserController(
            IDynamoDBContext dynamoDBContext,
            ILogger<UserController> logger)
        {
            _dbService = new DbService<User>(dynamoDBContext);
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<User>> GetByID(string id = "123")
        {
            return await _dbService.GetByID(id);
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
            var scan = await _dbService.dbContext.ScanAsync<User>(conditions).GetRemainingAsync();
            User newUser = new User()
            {
                UserName = inputUser.UserName,
                Email = inputUser.Email,
                firstName = inputUser.firstName,
                lastName = inputUser.lastName,
                Password = inputUser.Password
            };
            List<int> ids = new List<int>();
            for (int i = 0; i < scan.Count; i++)
            {
                ids.Add(Int32.Parse(scan[i].id));
            }

            for (int i = 0; i <= scan.Count + 1; i++)
            {
                if (!ids.Contains(i))
                {
                    /** assign ID **/
                    newUser.id = i.ToString();
                    await _dbService.dbContext.SaveAsync<User>(newUser);
                    return;
                }
            }

        }

        [HttpDelete]
        [Route("Delete")]
        public async Task DeleteByID(string id = "123")
        {
            /** For some reason, we can not just delete by the primary key directly
             * We have to load the object by ID, and then store it and delete it **/
            List<User> tmp = await _dbService.dbContext.QueryAsync<User>(id).GetRemainingAsync();
            if (tmp.Count > 0)
            {
                await _dbService.dbContext.DeleteAsync<User>(tmp[0]);
            }
        }

        
    }
}