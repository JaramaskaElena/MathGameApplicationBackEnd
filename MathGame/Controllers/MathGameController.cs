using MathGameApplication.Models;
using MathGameApplication.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace MathGameApplication.Controllers
{
    [ApiController]
    [Route("api/MathGame")]
    public class MathGameController : ControllerBase
    {

        private readonly IMathGameRepository _mathGameRepository;

        public MathGameController(IMathGameRepository mathGameRepository)
        {
            _mathGameRepository = mathGameRepository;
        }
        [HttpGet]
        [Route("users")]
        public async Task<List<User>> Get()
        {
            var users = await _mathGameRepository.GetUsers();
            return users;
        }
        [HttpGet]
        [Route("rounds/{id:long}")]
        public async Task<IActionResult> GetRounds(long id)
        {
            var rounds = await _mathGameRepository.GetRounds(id);
            return Ok(rounds);
        }
        [HttpPost]
        [Route("addUser")]
        public async Task<User> Post()
        {
            User user = new User()
            {
                LoginDate = DateTime.UtcNow,

            };
            using (var scope = new TransactionScope())
            {

                await _mathGameRepository.InsertUser(user);
                scope.Complete();
                return user;
            }
        }
        [HttpGet]
        [Route("activeUsers")]
        public async Task<int> GetActiveUsers()
        {
            var activeUsers = await _mathGameRepository.GetActiveUsers();
            return activeUsers;
        }
        [HttpGet]
        [Route("totalScoreByUser/{id:long}")]
        public async Task<int> GetTotalScoreByUser(long id)
        {
            var countUsers = await _mathGameRepository.GetTotalScorePerUser(id);
            return countUsers;
        }
        [HttpPut]
        [Route("update/{id:long}")]
        public void Put(long id, [FromBody] RequestModel model)
        {
            if (id != null && model.Answer != null)
            {
                using (var scope = new TransactionScope())
                {
                   _mathGameRepository.UpdateRoundForExpressionWithoutQusetionMark(id, model);
                    scope.Complete();
                }
            }
        }
        [HttpPut]
        [Route("updateRound/{id:long}")]
       public void Update(long id, RequestModel model)
        {
            if (id != null && model.Answer != null)
            {
                using (var scope = new TransactionScope())
                {
                    _mathGameRepository.UpdateRoundForExpressionWithQusetionMark(id, model);
                    scope.Complete();
                }
            }
        }
    }
}
