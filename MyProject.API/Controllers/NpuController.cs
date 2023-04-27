using Microsoft.AspNetCore.Mvc;
using MyProject.Model.Database;
using MyProject.Model.DTOs;
using MyProject.Model.Services;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NpuController : ControllerBase
    {
        private readonly IDatabaseService _service;

        public NpuController(IDatabaseService service)
        {
            _service = service;
        }

        //GET: api/Npu/1
        [HttpGet("{id}")]
        public ActionResult<NpuDTO> GetNpuById(int id) => (NpuDTO)_service.GetNpu(id);

        //GET: api/Npu
        [HttpGet]
        public ActionResult<IEnumerable<NpuDTO>> GetAllNpu() => _service.GetAllNpu().Select(c => (NpuDTO)c).ToList();

        //GET: api/Npu/name/elementName
        [HttpGet("name/{elementName}")]
        public ActionResult<IEnumerable<NpuDTO>> GetAllNpuBasedOnElement(string elementName)
            => _service.GetAllNpuBasedOnElement(elementName).Select(c => (NpuDTO)c).ToList();
        
        //POST: api/Npu/1
        [HttpPost("{userId}")]
        public IActionResult PostNpu(int userId, NpuDTO npu)
        {
            Npu newNpu = (Npu)npu;
            Npu find = _service.GetNpu(npu.Id);

            if (find == null)
            {
                var result = _service.AddNpu(newNpu, userId);
            }

            return CreatedAtAction("PostNpu", new { id = newNpu.Id }, (NpuDTO)newNpu);
        }

        //DELETE: api/Npu/1
        [HttpDelete("{id}")]
        public IActionResult DeleteNpu(int id)
        {
            Npu npu = _service.GetNpu(id);

            if (npu == null)
            {
                return NotFound();
            }
                
            _service.DeleteNpu(id);

            return Ok();
        }

        //PUT: api/Npu/score
        [HttpPut("score")]
        public IActionResult ScoreNpu(ScoreDTO scoreDto)
        {
            Npu npu = _service.GetNpu(scoreDto.ScoredNpuId);

            if (npu != null)
            {
                _service.ScoreNpu(scoreDto);
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
