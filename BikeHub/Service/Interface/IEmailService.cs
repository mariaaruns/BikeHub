namespace BikeHub.Service.Interface
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
