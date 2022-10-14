using PetHelper.Domain.Pets;

namespace PetHelper.Domain
{
    public record UserModel : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public IEnumerable<PetModel> Pets { get; set; }
    }
}
