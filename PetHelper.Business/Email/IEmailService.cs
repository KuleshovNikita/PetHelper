using PetHelper.Domain;

namespace PetHelper.Business.Email
{
    public interface IEmailService
    {
        Task SendEmailConfirmMessage(UserModel userModel);
    }
}
