namespace BikeHub.Repository.IRepository
{
    public interface IEmailRepository
    {
        Task<(string Subject, string HtmlBody)> GetEmailTemplateBySlug(string slugName);
    }
}
