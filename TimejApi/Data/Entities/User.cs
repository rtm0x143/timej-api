﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimejApi.Data.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        UNSPECIFIED,
        MALE,
        FEMALE
    }

    [Index(nameof(Email), IsUnique = true)]
    public record User
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Role
        {
            UNSPECIFIED,
            STUDENT,
            TEACHER,
            SCHEDULE_EDITOR,
            MODERATOR
        }
        public Guid Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [EmailAddress]
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? MiddleName { get; set; }
        public Gender Gender { get; set; }
        public ICollection<UserRole> Roles { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        //Student specific
        public Group? StudentGroup { get; set; }

        //Editor specific
        public ICollection<Faculty>? AllowedFaculties { get; set; }
    }
}
