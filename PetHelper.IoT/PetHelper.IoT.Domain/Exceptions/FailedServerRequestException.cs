namespace PetHelper.IoT.Domain.Exceptions
{
    public class FailedServerRequestException : Exception
    {
        public FailedServerRequestException()
        {

        }

        public FailedServerRequestException(string message) : base(message)
        {

        }
    }
}
