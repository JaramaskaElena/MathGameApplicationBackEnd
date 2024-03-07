using MathGameApplication.DbContexts;
using MathGameApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MathGameApplication.Repository
{
        public class MathGameRepository : IMathGameRepository
    {
        private readonly MathGameContext _dbContext;

        public MathGameRepository(MathGameContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Round>>GetRounds(long userId)
        {
            var rounds = await _dbContext.Rounds.ToListAsync();
            foreach (var round in rounds)
            {
                if(round.UserId != null && round.UserId!=userId)
                {
                    round.AnsweredValue = "MISSED";
                    round.Result = "FAILD";
                }
            }
            return rounds;
        }

        public async Task<int> GetActiveUsers()
        {
            var users = await GetUsers();
            return users.Where(x => x.LogoutDate == null).Count();
        }
        public async Task<int> GetTotalScorePerUser(long user)
        {
            var rounds = await _dbContext.Rounds.ToListAsync();
            var totalScore = 0;
            foreach (Round round in rounds)
            {
                if (round.Result != null)
                {
                    if (round.Result.Equals("OK") && round.UserId.Equals(user))
                    {
                        totalScore++;
                    }
                }
            }
            return totalScore;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task <User> InsertUser(User user)
        {
             _dbContext.Add(user);
            Save();
            return user;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public async void UpdateRoundForExpressionWithQusetionMark(long roundId, RequestModel model)
        {
            var entity = _dbContext.Rounds.FirstOrDefault(item => item.Id == roundId);
            var result = await GetResultForExpresionWithQuestionMark(roundId, model.Answer);
            if (entity != null && entity.UserId == null)
            {
                entity.UserId = model.UserId;
                entity.AnsweredValue = model.Answer.ToString();
                entity.Result = result ? "OK" : "FAILD";
            }
            var res = _dbContext.Rounds.Update(entity);
            Save();
        }

        public async void UpdateRoundForExpressionWithoutQusetionMark(long roundId, RequestModel model)
        {
            var entity = _dbContext.Rounds.FirstOrDefault(item => item.Id == roundId);
            var result = await GetResultForExpressionWithoutQuestionMark(roundId, model.Answer);
            if (entity != null && entity.UserId != null)
            {
                entity.Result = "FAILED";
                entity.AnsweredValue = "MISSED";
            }
            else if (entity != null && entity.UserId == null)
            {
                entity.UserId = model.UserId;
                entity.AnsweredValue = model.Answer.ToString();
                entity.Result = result ? "OK" : "FAILD";
            }
            var res = _dbContext.Rounds.Update(entity);
            Save();
        }
        private double GetCorrectAnswer(string expression)
        {
            var operand = expression[1];
            var parts = expression.Split('=');
            var leftFromEqual = parts[0].Split(new char[] { '+', '-','*','/' });
            var num1 = int.Parse(leftFromEqual[0]);
            var num2 = int.Parse(leftFromEqual[1]);
            if (num1 == 10)
            {
                operand = expression[2];
            }
            var result = GetOperandFromExpression(operand, num1, num2);
            return result;
        }
        private async Task<string> GetExpressionFromRoundId(long roundId)
        {
            var round =  _dbContext.Rounds.FirstOrDefaultAsync(x => x.Id == roundId);
            return round.Result.Expression;
        }
        private async Task<bool> GetResultForExpresionWithQuestionMark(long roundId, string answer)
        {
            var expression = await GetExpressionFromRoundId(roundId);
            var correctAnswer = GetCorrectAnswer(expression);
                if (int.Parse(answer) == correctAnswer)
                {
                    return true;
                }
                return false;
            }

        private async Task<bool> GetResultForExpressionWithoutQuestionMark(long roundId, string answer)
        {
            var result = "NO";
            var expression = await GetExpressionFromRoundId(roundId);
            var correctAnswer = GetCorrectAnswer(expression);
            var parts = expression.Split('=');
                var expResult = int.Parse(parts[1]);
                if (expResult == correctAnswer)
                {
                    result = "YES";
                }
                if (result == answer)
                    return true;
                return false;
            }


        private double GetOperandFromExpression(char operand,int num1, int num2)
        {
            var result = 0;
            switch (operand)
            {
                case '+': 
                    result = num1 + num2;
                    break;
                case '-':
                    result = num1 - num2;
                    break;
                case '*':
                    result = num1 * num2;
                    break;
                case '/':
                    result = num1 / num2;
                    break;
            }
            return result;
        }
    }
}