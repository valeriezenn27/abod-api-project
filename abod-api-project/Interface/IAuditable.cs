using System;
namespace abod_api_project.Interface
{
	public interface IAuditable
	{
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}

