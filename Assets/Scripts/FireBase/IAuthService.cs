public interface IAuthService
{
    System.Threading.Tasks.Task<string> Login(string email, string password);
    System.Threading.Tasks.Task<string> Register(string email, string password, string username);
    System.Threading.Tasks.Task SendPasswordResetEmail(string email);
    void Logout();
    string GetCurrentUserId();
}