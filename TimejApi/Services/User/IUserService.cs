using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TimejApi.Data;
using TimejApi.Helpers.Types;
using TimejApi.Data.Entities;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User
{
    public interface IUserService : IDbContextWrap<ScheduleDbContext>
    {
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> ChangePassword(Guid id, ChangePasswordDto change);
        public ValueTask<UnsuredResult<UserEntity, ModelStateDictionary>> TryLogin(UserLogin userLogin);
        public ValueTask<UserEntity?> TryGet(Guid id);
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryEdit(Guid id, UserEditDto edit);
        public ValueTask<UserEntity[]> QueryUsers(Guid? groudId, string? email, UserEntity.Role? role);

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Result is set of allowed after operation faculties</returns>
        public ValueTask<UnsuredResult<IEnumerable<Faculty>, Exception>> TryGrantEditPermission(Guid userId, Guid facultyId);
        /// <exception cref="KeyNotFoundException"></exception>
        public ValueTask<UnsuredResult<UserEditFacultyPermission, Exception>> RevokeEditPermission(Guid userId, Guid facultyId);
        public ValueTask<UserEntity> Register(UserRegister userRegister);
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryChangeUser(Guid id, UserRegister userRegister);
    }
}
