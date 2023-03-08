using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
using TimejApi.Data.Entities;
using TimejApi.Helpers.Types;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User;

public class EditPermissonService : IEditPermissonService
{
    public ScheduleDbContext DbContext { get; private init; }
    DbContext IDbContextWrap.DbContext => DbContext;

    public EditPermissonService(ScheduleDbContext context)
    {
        DbContext = context;
    }

    public ValueTask<UnsuredResult<Faculty[], Exception>> TryGetEditPermissions(Guid userId)
    {
        var task = DbContext.Users.Where(u => u.Id == userId)
                .Include(nameof(UserEntity.AllowedFaculties))
                .Select(u => u.AllowedFaculties)
                .FirstOrDefaultAsync()
                .ContinueWith<UnsuredResult<Faculty[], Exception>>(t =>
                {
                    if (t.Result != null) return new(t.Result.ToArray());
                    return new(new KeyNotFoundException(nameof(userId)));
                });

        return new (task);
    }

    public async ValueTask<UnsuredResult<Faculty[], Exception>> TryGrantEditPermission(Guid userId, Guid facultyId)
    {
        var user = await DbContext.Users.Include(nameof(UserEntity.AllowedFaculties))
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return new(new KeyNotFoundException(nameof(userId)));

        if ((await DbContext.Faculties.FindAsync(facultyId)) is not Faculty faculty)
            return new(new KeyNotFoundException(nameof(facultyId)));

        if (user.AllowedFaculties!.FirstOrDefault(af => af.Id == facultyId) != null)
            return new(new ArgumentException("That relation already exist"));

        user.AllowedFaculties!.Add(faculty);
        await DbContext.SaveChangesAsync();
        return new(user.AllowedFaculties.ToArray());
    }

    public async ValueTask<UnsuredResult<UserEditFacultyPermission, Exception>> RevokeEditPermission(Guid userId, Guid facultyId)
    {
        var perm = await DbContext.UserEditFacultyPermissions.FindAsync(userId, facultyId);
        if (perm == null) return new(new KeyNotFoundException());
        DbContext.UserEditFacultyPermissions.Remove(perm);
        await DbContext.SaveChangesAsync();
        return new(perm);
    }
}
