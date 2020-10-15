namespace EFCore.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EFCore.Users.Model;

    using Lib.Users.Domain;
    using Lib.Users.Interfaces;

    using DbUser = Model.User;
    using User = Lib.Users.Domain.User;

    public class UsersEntityFrameworkRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UsersEntityFrameworkRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public ResultDto<IEnumerable<User>> GetUsers()
        {
            try
            {
                var users = QueryForUsers();

                var results = ConvertDbUsersToResultUsers(users);

                return new ResultDto<IEnumerable<User>> { Results = results, Success = false };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResultDto<IEnumerable<User>> { Results = null, Success = false };
            }
        }

        public ResultDto<User> InsertUser(User user)
        {
            try
            {
                _userContext.Add(new DbUser { Name = user.Name, Cell = user.Cell, Email = user.Email, Phone = user.Phone });
                _userContext.SaveChanges();

                return new ResultDto<User> { Results = user, Success = false };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ResultDto<User> { Results = null, Success = false };
            }
        }

        private List<User> ConvertDbUsersToResultUsers(IOrderedQueryable<DbUser> users)
        {
            return users.Select(item => new User { Name = item.Name, Cell = item.Cell, Email = item.Email, Phone = item.Phone }).ToList();
        }

        private IOrderedQueryable<DbUser> QueryForUsers()
        {
            var users = from user in _userContext.Users orderby user.UserId select user;
            return users;
        }
    }
}