﻿namespace PetHelper.Api.RequestModels
{
    public record UserRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string Password { get; set; }

        public string Login { get; set; }
    }
}
