
namespace Tester.Models
{
    public class UserAccount
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public long SingleEasyWins { get; set; }
        public long SingleEasyLoses { get; set; }
        public long SingleEasyTies { get; set; }
        public long SingleHardWins { get; set; }
        public long SingleHardLoses { get; set; }
        public long SingleHardTies { get; set; }
        public long DoubleWins { get; set; }
        public long DoubleLoses { get; set; }
        public long DoubleTies { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
