namespace PetHelper.Domain.Exceptions
{
    public class NoIdleDataForAnimalExistsException : Exception
    {
        public NoIdleDataForAnimalExistsException()
        {

        }

        public NoIdleDataForAnimalExistsException(string message) : base(message)
        {

        }
    }
}
