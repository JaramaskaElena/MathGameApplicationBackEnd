namespace MathGameApplication.Models
{
    public class UserRoundRequestModel
    {
        public long Id { get; set; }
        public string Expression { get; set; }
        public long UserId { get; set; }
        public string AnsweredValue { get; set; }
    }
}