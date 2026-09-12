using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.API.Models;
using SmartRecruitment.API.Services.Interfaces;

namespace SmartRecruitment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<List<Skill>>> GetAll()
        {
            var skills = await _skillService.GetAllAsync();

            return Ok(skills);
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetById(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);

            if (skill == null)
                return NotFound(new { message = "Skill not found." });

            return Ok(skill);
        }

        // GET: api/Skills/name/CSharp
        [HttpGet("name/{skillName}")]
        public async Task<ActionResult<Skill>> GetByName(string skillName)
        {
            var skill = await _skillService.GetByNameAsync(skillName);

            if (skill == null)
                return NotFound(new { message = "Skill not found." });

            return Ok(skill);
        }

        // POST: api/Skills
        [HttpPost]
        public async Task<ActionResult<Skill>> Create(Skill skill)
        {
            if (string.IsNullOrWhiteSpace(skill.SkillName))
                return BadRequest(new { message = "Skill name is required." });

            var exists = await _skillService.ExistsByNameAsync(skill.SkillName);

            if (exists)
                return Conflict(new { message = "Skill already exists." });

            var createdSkill = await _skillService.AddAsync(skill);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdSkill.SkillId },
                createdSkill);
        }

        // PUT: api/Skills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Skill skill)
        {
            if (id != skill.SkillId)
                return BadRequest(new { message = "Skill ID mismatch." });

            var exists = await _skillService.ExistsByIdAsync(id);

            if (!exists)
                return NotFound(new { message = "Skill not found." });

            if (string.IsNullOrWhiteSpace(skill.SkillName))
                return BadRequest(new { message = "Skill name is required." });

            await _skillService.UpdateAsync(skill);

            return NoContent();
        }

        // DELETE: api/Skills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _skillService.ExistsByIdAsync(id);

            if (!exists)
                return NotFound(new { message = "Skill not found." });

            await _skillService.DeleteAsync(id);

            return NoContent();
        }
    }
}