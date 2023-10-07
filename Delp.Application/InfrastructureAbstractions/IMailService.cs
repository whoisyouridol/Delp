namespace Delp.Application.InfrastructureAbstractions
{
    public interface IMailService
    {
        Task SendMail(IEnumerable<string> to, string subject, string content);
    }
}
