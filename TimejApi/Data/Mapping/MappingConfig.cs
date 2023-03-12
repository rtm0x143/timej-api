using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
                .Map(dest => dest.Roles, src => src.Roles.Select(r => r.Role))
                .Map(dest => dest.Group, src => src.StudentGroup);


            TypeAdapterConfig<UserDto, User>.NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(r => new UserRole() { UserId = src.Id, Role = r }))
                .Map(dest => dest.StudentGroup, src => src.Group);

            TypeAdapterConfig<UserData, User>.NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(r => new UserRole { Role = r }))
                .Ignore(u => u.AllowedFaculties!);

            TypeAdapterConfig<UserRegister, User>.NewConfig()
                .Ignore(u => u.AllowedFaculties!)
                .Ignore(u => u.StudentGroup!.Lessons!)
                .Ignore(u => u.StudentGroup!.Faculty)
                .Map(dest => dest.Roles, src => src.Roles.Select(r => new UserRole() { Role = r }))
                .Map(dest => dest.StudentGroup, src => src.Group);

            TypeAdapterConfig<LessonCreation, Lesson>.NewConfig()
                .Map(dest => dest.LessonNumber, src => src.LessonNumber);

            TypeAdapterConfig<Lesson, LessonDto>.NewConfig()
                .Map(dest => dest.LessonNumber, src => src.LessonNumber)
                .Map(dest => dest.Groups, 
                    src => src.AttendingGroups.Select(x => 
                        new SubgroupDto(x.GroupId, x.SubgroupNumber, x.Group != null ? x.Group!.GroupNumber : default)));

            TypeAdapterConfig<LessonGroup, SubgroupDto>.NewConfig()
                .Map(dest => dest.GroupNumber, src => src.Group.GroupNumber)
                .Map(dest => dest.Id, src => src.GroupId);

        }
    }
}
