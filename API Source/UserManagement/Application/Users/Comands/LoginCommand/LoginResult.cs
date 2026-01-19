namespace UserManagement.Application.Users.Comands.LoginCommand
{
    public class LoginResult
    {
        public bool IsLoginSuccessful { get; set; }

        public string JwtToken { get; set; }

        public string Message { get; set; }
    }
}
