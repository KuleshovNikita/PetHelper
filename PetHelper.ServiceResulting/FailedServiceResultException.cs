namespace PetHelper.ServiceResulting
{
    public class FailedServiceResultException : Exception
    {
        public FailedServiceResultException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
