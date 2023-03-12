using TimejApi.Data;
using TimejApi.Helpers.Types;
using TimejApi.Data.Entities;

namespace TimejApi.Services.User
{
    public interface IEditPermissonService : IDbContextWrap<ScheduleDbContext>
    {
        /// <exception cref="KeyNotFoundException"></exception>
        public ValueTask<Result<Faculty[], Exception>> TryGetEditPermissions(Guid userId);

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>Result is set of allowed after operation faculties</returns>
        public ValueTask<Result<Faculty[], Exception>> TryGrantEditPermission(Guid userId, Guid facultyId);

        /// <exception cref="KeyNotFoundException"></exception>
        public ValueTask<Result<UserEditFacultyPermission, Exception>> RevokeEditPermission(Guid userId, Guid facultyId);
    }


}
