using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TimejApi.Data;
using TimejApi.Helpers.Types;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User
{
    public interface IUserService : IDbContextWrap<ScheduleDbContext>
    {
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryChangeUser(Guid id, UserRegister userRegister);
        public ValueTask<UserEntity> Register(UserRegister userRegister);
        public ValueTask<UnsuredResult<UserEntity, ModelStateDictionary>> TryLogin(UserLogin userLogin);
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> ChangePassword(Guid id, ChangePasswordDto change);
        public ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryEdit(Guid id, UserEditDto edit);
        public ValueTask<UserEntity?> TryGet(Guid id);
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
            var user = await DbContext.Users
                .Include(u => u.Roles)
                .Include(u => u.StudentGroup)
                .FirstOrDefaultAsync(u => u.Email == userLogin.Email);

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

        private EntityEntry<UserEntity> _translateUserRegister(UserRegister userRegister)
        {
            var entry = DbContext.Users.Entry(userRegister.Adapt<UserEntity>());
            if (userRegister.Group != null)
            {
                var studentGroupEntry = entry.Reference(nameof(UserEntity.StudentGroup));
                studentGroupEntry.EntityEntry.State = EntityState.Unchanged;
                studentGroupEntry.Load();
            }
            entry.Entity.PasswordHash = _passwordHasher.HashPassword(userRegister.Password);
            return entry;
        }

        /// <summary>
        /// Creates new User entity from <see cref="UserRegister"/> Dto
        /// </summary>
        public async ValueTask<UserEntity> Register(UserRegister userRegister)
        {
            var entry = _translateUserRegister(userRegister);
            entry.State = EntityState.Added;
            await DbContext.SaveChangesAsync();

            return entry.Entity;
        }

        public async ValueTask<UnsuredResult<UserEntity, ProblemDetails>> ChangePassword(Guid userId, ChangePasswordDto change)
        {
            var user = await TryGet(userId);
            if (user == null)
                return UnsuredDetailedResult.NotFound<UserEntity>();

            if (user.PasswordHash == null || !_passwordHasher.VerifyPassword(change.OldPassword, user.PasswordHash))
                return new(new ProblemDetails() { Status = StatusCodes.Status403Forbidden, Title = "Incorrect password" });

            user.PasswordHash = _passwordHasher.HashPassword(change.NewPassword);
            await DbContext.SaveChangesAsync();
            return new(user);
        }

        public async ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryEdit(Guid id, UserEditDto edit)
        {
            var user = await DbContext.Users.FindAsync(id);
            if (user == null) return UnsuredDetailedResult.NotFound<UserEntity>();

            user.Email = edit.Email;
            await DbContext.SaveChangesAsync();
            return new(user);
        }

        public async ValueTask<UnsuredResult<UserEntity, ProblemDetails>> TryChangeUser(Guid id, UserRegister userRegister)
        {
            var user = await TryGet(id);
            if (user == null) return UnsuredDetailedResult.NotFound<UserEntity>();

            // Hugely frustrated that i should do that for EF; solution could be using specific Dto mapper            
            user.Gender = userRegister.Gender;
            user.Email = userRegister.Email;
            user.MiddleName = userRegister.MiddleName;
            user.Name = userRegister.Name;
            user.Surname= userRegister.Surname;
            user.PasswordHash = _passwordHasher.HashPassword(userRegister.Password);
            
            if (userRegister.Group?.Id != user.StudentGroup?.Id)
                user.StudentGroup = userRegister.Group?.Adapt<Data.Entities.Group>();
            
            await DbContext.SaveChangesAsync();
            return new(user);
        }

        public ValueTask<UserEntity?> TryGet(Guid id) => new(
            DbContext.Users
                .Include(u => u.Roles)
                .Include(u => u.StudentGroup)
                .FirstOrDefaultAsync(u => u.Id == id));
    }
}