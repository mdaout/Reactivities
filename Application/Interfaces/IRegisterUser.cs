using System.Threading.Tasks;
using Domain;
//using static Application.User.RegisterUser;

namespace Application.Interfaces
{
    public interface IRegisterUser
    {
           Task<Application.User.User> AddIt (AddUser request);
    }
}