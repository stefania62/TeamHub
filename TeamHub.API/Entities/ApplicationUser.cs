﻿using Microsoft.AspNetCore.Identity;
using TeamHub.API.Models;

namespace TeamHub.API.Entities
{
    /// <summary>
    /// Custom User model for authentication.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public ICollection<ProjectEmployee> Projects { get; set; } = new List<ProjectEmployee>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
