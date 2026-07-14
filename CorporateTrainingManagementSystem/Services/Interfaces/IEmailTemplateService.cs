namespace CorporateTrainingManagementSystem.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GetConfirmationEmailTemplate(string confirmationLink);

        string GetResetPasswordTemplate(string resetPasswordLink);
    }
}
