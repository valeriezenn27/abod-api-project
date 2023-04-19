using System;
namespace abod_api_project.Interface
{
	public interface ISoftDeletable
    {
        bool Deleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}

