using MathGameApplication.Models;

namespace MathGameApplication.Repository
{
    public interface IMathGameRepository
    {
        Task<List<Round>> GetRounds(long userId);
        Task<List<User>> GetUsers();
        Task<int> GetActiveUsers();
        Task<User> InsertUser(User user);
        void UpdateRoundForExpressionWithoutQusetionMark(long roundId, RequestModel model);
        void UpdateRoundForExpressionWithQusetionMark(long roundId, RequestModel model);
        Task<int> GetTotalScorePerUser(long userId);
        void Save();
    }
}