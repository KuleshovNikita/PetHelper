using PetHelper.IoT.Domain.PetModels;

namespace PetHelper.IoT.Domain.UserModels
{
    public record UserModel : BaseModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Age { get; set; }

        public IEnumerable<PetModel>? Pets { get; set; }

        public string Password { get; set; } = null!;

        public string Login { get; set; } = null!;

        public bool IsEmailConfirmed { get; set; }
    }
}
