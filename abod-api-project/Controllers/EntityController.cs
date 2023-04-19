using abod_api_project.Exceptions;
using abod_api_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MyController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var myEntity = await _dbContext.MyEntities.FindAsync(id);
            if (myEntity == null)
            {
                throw new CustomException($"MyEntity with ID {id} not found.");
            }
            return Ok(myEntity);
        }
        catch (CustomException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Entity entity)
    {
        try
        {
            var myEntity = new Entity
            {
                Name = entity.Name,
                CreatedBy = "User1", // You can set the CreatedBy and CreatedAt properties here
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.MyEntities.Add(myEntity);
            await _dbContext.SaveChangesAsync();
            return Ok(myEntity);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Entity dto)
    {
        try
        {
            var myEntity = await _dbContext.MyEntities.FindAsync(id);
            if (myEntity == null)
            {
                throw new CustomException($"MyEntity with ID {id} not found.");
            }
            myEntity.Name = dto.Name;
            myEntity.UpdatedBy = "User2"; // You can set the UpdatedBy and UpdatedAt properties here
            myEntity.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return Ok(myEntity);
        }
        catch (CustomException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var myEntity = await _dbContext.MyEntities.FindAsync(id);
            if (myEntity == null)
            {
                throw new CustomException($"MyEntity with ID {id} not found.");
            }
            myEntity.Deleted = true;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (CustomException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
