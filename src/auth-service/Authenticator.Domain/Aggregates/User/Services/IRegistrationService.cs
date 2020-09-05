﻿namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface IRegistrationService
    {
        void CreateAccount(Account account);

        bool VerifyEmail(string email);
    }
}