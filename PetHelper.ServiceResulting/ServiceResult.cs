﻿using System.Text.Json.Serialization;

namespace PetHelper.ServiceResulting
{
    public class ServiceResult<TEntity>
    {
        public bool IsSuccessful { get; set; } = true;

        public TEntity Value { get; set; }

        [JsonIgnore]
        public Exception Exception { get; private set; } = new Exception();

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
            Fail(failMessage);

            throw new FailedServiceResultException(failMessage, Exception);
        }

        public ServiceResult<TEntity> Fail(string failMessage = "") 
            => SetResultState(false, failMessage);

        private ServiceResult<TEntity> SetResultState(bool state, string message)
        {
            ClientErrorMessage = message;
            IsSuccessful = state;
            return this;
        }
    }
}