namespace Lib.Users.Interfaces
{
    using System.Collections.Generic;

    using Lib.Users.Domain;

    public interface IUserRepository
    {
        ResultDto<IEnumerable<User>> GetUsers();

        ResultDto<User> InsertUser(User user);
    }
}