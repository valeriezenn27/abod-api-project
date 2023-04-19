using System;
namespace abod_api_project.Models
{
    public interface IEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        bool Deleted { get; set; }
    }
}

