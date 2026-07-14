namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GetConfirmationEmailTemplate(string confirmationLink)
        {
            return $@"
                <h2>Welcome to Corporate Training Management System</h2>

                <p>
                    Please confirm your email by clicking the button below.
                </p>

                <p>
                    <a href='{confirmationLink}'
                       style='padding:10px 20px;
                              background:#0d6efd;
                              color:white;
                              text-decoration:none;
                              border-radius:5px;'>
                        Confirm Email
                    </a>
                </p>

                <p>
                    If you did not create this account, you can safely ignore this email.
                </p>";
        }

        public string GetResetPasswordTemplate(string resetPasswordLink)
        {
            return $@"
                <h2>Reset Your Password</h2>

                <p>
                    Click the button below to reset your password.
                </p>

                <p>
                    <a href='{resetPasswordLink}'
                       style='padding:10px 20px;
                              background:#dc3545;
                              color:white;
                              text-decoration:none;
                              border-radius:5px;'>
                        Reset Password
                    </a>
                </p>

                <p>
                    If you did not request a password reset, you can safely ignore this email.
                </p>";
        }
    }
}