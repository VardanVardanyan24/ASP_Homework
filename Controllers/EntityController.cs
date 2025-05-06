using ASP_Homework;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/entity")]
public class EntityController : ControllerBase
{
    [HttpPost("edit")]
    public IActionResult EditEntity([FromBody] EditEntityDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        EntityStorage.Username = dto.Username;
        EntityStorage.Email = dto.Email;
        EntityStorage.Password = dto.Password;
        EntityStorage.DateOfBirth = dto.DateOfBirth;
        EntityStorage.Quantity = dto.Quantity;
        EntityStorage.Price = decimal.TryParse(dto.Price, out var price) ? price : null;
        EntityStorage.Amount = dto.Amount;

        return Ok("Entity updated successfully.");
    }
}