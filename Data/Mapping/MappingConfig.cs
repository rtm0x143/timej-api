using Mapster;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Data.Mapping
{
    public static class MappingConfig
    {
        public static void Apply()
        {
            // Add mapping detail like this if found some 
            //
            //  TypeAdapterConfig<User, UserDto>.NewConfig()
            //      .Map(dest => , src => )
            //      .TwoWays()

            TypeAdapterConfig<User, Teacher>.NewConfig()
                .Map(dest => dest.Fullname, src => $"{src.Surname} {src.Name} {src.MiddleName}");

            TypeAdapterConfig<User, UserDto>.NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(r => r.Role));

            TypeAdapterConfig<UserData, User>.NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(r => new UserRole { Role = r }))
                .Ignore(u => u.AllowedFaculties!);

            TypeAdapterConfig<UserRegister, User>.NewConfig()
                .Ignore(u => u.AllowedFaculties!)
                .Ignore(u => u.StudentGroup!.Lessons!)
                .Ignore(u => u.StudentGroup!.Faculty)
                .Map(dest => dest.Roles, src => src.Roles.Select(r => new UserRole() { Role = r }))
                .Map(dest => dest.StudentGroup, src => src.Group != null ? new Group() { GroupNumber = src.Group.GroupNumber } : null);

        }
    }
}
