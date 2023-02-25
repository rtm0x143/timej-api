using TimejApi.Data;
using TimejApi.Data.Dtos;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User
{
    public interface IUserService
    {
        public UserEntity Register(UserRegister userRegister);
        public UserEntity Login(UserLogin userLogin);
        public UserEntity Update(UserDto userDto);
    }

    public class UserService : IUserService
    {
        private readonly ScheduleDbContext _context;

        public UserService(ScheduleDbContext context)
        {
            _context = context;
        }


        public void GrantFacultyEditor(IEnumerable<Guid> facultyIds)
        {
            throw new NotImplementedException();
        }

        public UserEntity Login(UserLogin userLogin)
        {
            throw new NotImplementedException();
        }

        public UserEntity Register(UserRegister userRegister)
        {
            throw new NotImplementedException();
        }

        public UserEntity Update(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
