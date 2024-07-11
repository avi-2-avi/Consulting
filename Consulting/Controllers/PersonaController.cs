using Consulting.Models;
using Consulting.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Consulting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonaController: ControllerBase
{
    private readonly PersonaRepository _repository;

    public PersonaController(PersonaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
    {
        return Ok(await _repository.GetPersonasAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetPersona(int id)
    {
        var persona = await _repository.GetPersonaByIdAsync(id);

        if (persona == null)
        {
            return NotFound();
        }

        return Ok(persona);
    }

    [HttpPost]
    public async Task<ActionResult> AddPersona(Persona persona)
    {
        await _repository.AddPersonaAsync(persona);
        return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePersona(Persona persona)
    {
        await _repository.UpdatePersonaAsync(persona);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersona(int id)
    {
        await _repository.DeletePersonaAsync(id);
        return NoContent();
    }
}