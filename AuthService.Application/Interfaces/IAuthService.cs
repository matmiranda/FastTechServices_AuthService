namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        // Exemplo de assinatura de método
        Task<string> LoginAsync(string email, string password);
    }
}
