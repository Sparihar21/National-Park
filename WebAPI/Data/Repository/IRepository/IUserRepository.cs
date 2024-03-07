using WebAPI.Models;

namespace WebAPI.Data.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUserName (string userName);
        User Autehnticate(string userName, string Password);
        User Register(string userName, string Password);
    }
}
