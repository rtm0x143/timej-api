using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
using TimejApi.Helpers.Types;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User
{
    public interface IUserService : IDbContextWrap<ScheduleDbContext>
    {
        public ValueTask<UserEntity> Register(UserRegister userRegister);
        public ValueTask<UnsuredResult<UserEntity, ModelStateDictionary>> TryLogin(UserLogin userLogin);
        public ValueTask<bool> ChangePassword(UserEntity user, ChangePasswordDto change);
        public ValueTask<UserEntity> Edit(UserEntity user, UserEditDto edit);
    }

    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;

        public ScheduleDbContext DbContext { get; private init; }
        DbContext IDbContextWrap.DbContext => DbContext;

        public UserService(ScheduleDbContext context, IPasswordHasher passwordHasher)
        {
            DbContext = context;
            _passwordHasher = passwordHasher;
        }

        public async ValueTask<UnsuredResult<UserEntity, ModelStateDictionary>> TryLogin(UserLogin userLogin)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);
            if (user == null)
            {
                var state = new ModelStateDictionary();
                state.AddModelError(nameof(UserLogin.Email), "User with specified email is unknown");
                return new(state);
            }

            if (user.PasswordHash == null 
                || !_passwordHasher.VerifyPassword(userLogin.Password, user.PasswordHash))
            {
                var state = new ModelStateDictionary();
                state.AddModelError(nameof(UserLogin.Password), "Incorrect password");
                return new(state);
            }

            return new(user);
        }

        /// <summary>
        /// Creates new User entity from <see cref="UserRegister"/> Dto
        /// </summary>
        public async ValueTask<UserEntity> Register(UserRegister userRegister)
        {
            var userModel = userRegister.Adapt<UserEntity>();
            if (userModel.StudentGroup != null)
                DbContext.Groups.Entry(userModel.StudentGroup).State = EntityState.Unchanged;
            userModel.PasswordHash = _passwordHasher.HashPassword(userRegister.Password);

            var entry = DbContext.Users.Add(userModel);
            await DbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async ValueTask<bool> ChangePassword(UserEntity user, ChangePasswordDto change)
        {
            if (user.PasswordHash == null || !_passwordHasher.VerifyPassword(change.OldPassword, user.PasswordHash))
                return false;

            var changedUser = user with { PasswordHash = _passwordHasher.HashPassword(change.NewPassword) };
            DbContext.Users.Update(changedUser);
            await DbContext.AddRangeAsync(changedUser);
            return true;
        }

        public async ValueTask<UserEntity> Edit(UserEntity user, UserEditDto edit)
        {
            var newUserModel = user with { Email = edit.Email };
            DbContext.Users.Update(newUserModel);
            await DbContext.SaveChangesAsync();
            return newUserModel;
        }
    }
}