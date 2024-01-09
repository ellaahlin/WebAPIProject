using System;
using Models.DTO;

namespace Services
{
	public interface ILoginService
	{
        public Task<int> SeedUsersAsync(int nrOfUsers);
        public Task<loginUserSessionDto> LoginUserAsync(loginCredentialsDto usrCreds);
    }
}

