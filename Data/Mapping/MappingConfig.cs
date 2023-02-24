using Mapster;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Data.Mapping
{
    public static class MappingConfig
    {
        public static void Aplly()
        {
            // Add mapping detail like this if found some 
            //
            //  TypeAdapterConfig<User, UserDto>.NewConfig()
            //      .Map(dest => , src => )
            //      .TwoWays()

            TypeAdapterConfig<User, Teacher>.NewConfig()
                .Map(dest => dest.Fullname, src => $"{src.Surname} {src.Name} {src.MiddleName}");
        }
    }
}
