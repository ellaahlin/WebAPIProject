using System;
namespace Models
{
	public interface IUser
	{
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<IComment> Comments { get; set; }
        /* Gör om alla User-klasser så att det fungerar att seeda och det finns en koppling mellan comments och users
         */
    }
}

