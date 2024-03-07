using Microsoft.AspNetCore.SignalR;

namespace MathGameApplication.Models
{
    public class Round
    {
        public long Id { get; set; }
        public string Expression { get; set; }
        public string? AnsweredValue { get; set; }
        public string? Result { get; set; }
        public long? UserId { get; set; }
        public User User { get; set; }

    }
}
