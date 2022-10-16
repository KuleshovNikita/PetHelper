﻿using System.Text.Json.Serialization;

namespace PetHelper.ServiceResulting
{
    public class ServiceResult<TEntity>
    {
        public bool IsSuccessful { get; set; } = true;

        public TEntity Value { get; set; }

        [JsonIgnore]
        public Exception Exception { get; private set; }

        public string? ClientErrorMessage { get; private set; }

        public ServiceResult() { }

        public ServiceResult<TEntity> Catch<TException>(string? errorMessage = null) where TException : Exception
        {   
            if(Exception is TException)
            {
                ClientErrorMessage = errorMessage ?? Exception.Message;

                throw new FailedServiceResultException(ClientErrorMessage, Exception);
            }

            return this;
        }

        public ServiceResult<TEntity> Success() => SetResultState(true, string.Empty);

        public ServiceResult<TEntity> FailAndThrow(string failMessage = "")
        {
            var serviceFailedException = new FailedServiceResultException(failMessage, Exception);
            Fail(serviceFailedException);

            throw serviceFailedException;
        }

        public ServiceResult<TEntity> Fail(Exception ex)
            => SetResultState(false, ex.Message, ex);

        private ServiceResult<TEntity> SetResultState(bool state, string message, Exception? ex = null)
        {
            if(ex is not null)
            {
                Exception = ex.InnerException ?? ex;
            }
    
            ClientErrorMessage = message;
            IsSuccessful = state;
            return this;
        }
    }
}