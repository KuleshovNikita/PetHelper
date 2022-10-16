using PetHelper.Domain.Pets;

namespace PetHelper.Domain
{
    public record UserModel : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public IEnumerable<PetModel>? Pets { get; set; }

        public string Password { get; set; }

        public string Login { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
