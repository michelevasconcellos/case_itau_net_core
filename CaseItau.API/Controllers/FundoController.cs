using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CaseItau.API.Domain.Entities;
using CaseItau.API.Application.DTOs;
using CaseItau.API.Services;

namespace CaseItau.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundoController : ControllerBase
    {
        private readonly IFundoService _fundoService;

        public FundoController(IFundoService fundoService)
        {
            _fundoService = fundoService;
        }

        // GET: api/fundo
        [HttpGet]
        public ActionResult<IEnumerable<FundoResponse>> GetAll()
        {
            var fundos = _fundoService.GetAll();
            return Ok(fundos);
        }

        // GET: api/fundo/FND1001
        [HttpGet("{codigo}")]
        public ActionResult<FundoResponse> GetByCodigo(string codigo)
        {
            var fundo = _fundoService.GetByCodigo(codigo);

            if (fundo == null)
            {
                return NotFound(new
                {
                    message = "Fundo não encontrado."
                });
            }

            return Ok(fundo);
        }

        // POST: api/fundo
        [HttpPost]
        public IActionResult Post([FromBody] Fundo value)
        {
            _fundoService.Add(value);

            return CreatedAtAction(
                nameof(GetByCodigo),
                new { codigo = value.Codigo },
                value);
        }

        // PUT: api/fundo/FND1001
        [HttpPut("{codigo}")]
        public IActionResult Put(string codigo, [FromBody] Fundo value)
        {
            _fundoService.Update(codigo, value);

            return Ok(new
            {
                message = "Fundo atualizado com sucesso."
            });
        }

        // DELETE: api/fundo/FND1001
        [HttpDelete("{codigo}")]
        public IActionResult Delete(string codigo)
        {
            _fundoService.Delete(codigo);

            return Ok(new
            {
                message = "Fundo removido com sucesso."
            });
        }

        // PUT: api/fundo/FND1001/patrimonio
        [HttpPut("{codigo}/patrimonio")]
        public IActionResult UpdatePatrimonio(
            string codigo,
            [FromBody] decimal value)
        {
            _fundoService.UpdatePatrimonio(codigo, value);

            return Ok(new
            {
                message = "Patrimônio atualizado com sucesso."
            });
        }
    }
}