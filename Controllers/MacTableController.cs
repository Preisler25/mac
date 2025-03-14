using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mac.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MacTableController : ControllerBase
{
    private readonly MacTableDataContext db;

    public MacTableController(MacTableDataContext context)
    {
        db = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var macTableDatas = db.MacTableDatas.ToList();
        return Ok(macTableDatas);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var macTableData = db.MacTableDatas.Find(id);
        if (macTableData == null)
        {
            return NotFound();
        }
        return Ok(macTableData);
    }

    [HttpPost]
    public IActionResult Post([FromBody] CreateMacTableDataDto dto)
    {
        var macTableData = new MacTableData
        {
            MacAddress = dto.Address,
            Email = dto.Email,
            ExpirationDate = dto.ExpirationDate
        };

        var validator = new MacTableDataValidator(db);
        var result = validator.Validate(macTableData);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        db.MacTableDatas.Add(macTableData);
        db.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = macTableData.Id }, macTableData);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var macTableData = db.MacTableDatas.Find(id);
        if (macTableData == null)
        {
            return NotFound();
        }

        db.MacTableDatas.Remove(macTableData);
        db.SaveChanges();
        return NoContent();
    }
}
