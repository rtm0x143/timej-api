using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TimejApi.Data;
using TimejApi.Helpers.Types;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User
{
    public interface IUserService : IDbContextWrap<ScheduleDbContext>
    {
        public ValueTask<UserEntity?> TryGet(Guid id);
        public ValueTask<UserEntity[]> QueryUsers(Guid? groudId, string? email, UserEntity.Role? role);
        public ValueTask<UserEntity> Register(UserRegister userRegister);
        public ValueTask<UnsuredResult<UserEntity, ModelStateDictionary>> TryLogin(UserLogin userLogin);

        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> ChangePassword(Guid id, ChangePasswordDto change);
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryEdit(Guid id, UserEditDto edit);
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryChangeUser(Guid id, UserRegister userRegister);
    }
}
