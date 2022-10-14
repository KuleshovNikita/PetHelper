using System.Text.Json.Serialization;

namespace PetHelper.ServiceResulting
{
    public class ServiceResult
    {
        public bool IsSuccessful { get; private set; } = true;

        [JsonIgnore]
        public List<Exception> Exceptions { get; } = new List<Exception>();

        public string? ClientErrorMessage { get; private set; }

        private bool _continueCatching = true;

        public ServiceResult Execute(Func<ServiceResult> command)
        {
            try
            {
                if (IsSuccessful)
                {
                    command();
                }
            }
            catch(Exception ex)
            {
                IsSuccessful = false;
                Exceptions.Add(ex.InnerException ?? ex);
            }

            return this;
        }

        public async Task<ServiceResult> ExecuteAsync<T>(Func<Task<T>> command)
        {
            try
            {
                if (IsSuccessful)
                {
                    await command();
                }
            }
            catch (Exception ex)
            {
                IsSuccessful = false;
                Exceptions.Add(ex.InnerException ?? ex);
            }

            return this;
        }

        public ServiceResult Catch<TException>(string errorMessage) where TException : Exception
        {
            if(!IsSuccessful && Exceptions.Any(ex => ex is TException))
            {
                _continueCatching = false;
                ClientErrorMessage = errorMessage;
            }

            return this;
        }

        public ServiceResult Success() => SetResultState(true, string.Empty);

        public ServiceResult Fail(string failMessage = "") => SetResultState(false, failMessage);

        private ServiceResult SetResultState(bool state, string message)
        {
            ClientErrorMessage = message;
            IsSuccessful = state;
            return this;
        }
    }
}