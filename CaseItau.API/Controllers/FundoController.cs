using System;
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
        public IActionResult GetAll()
        {
            try
            {
                var fundos = _fundoService.GetAll();

                return Ok(new
                {
                    success = true,
                    data = fundos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro interno ao consultar os fundos.",
                    detail = ex.Message
                });
            }
        }

        // GET: api/fundo/FND1001
        [HttpGet("{codigo}")]
        public IActionResult GetByCodigo(string codigo)
        {
            try
            {
                var fundo = _fundoService.GetByCodigo(codigo);

                if (fundo == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Fundo não encontrado."
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = fundo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro interno ao consultar o fundo.",
                    detail = ex.Message
                });
            }
        }

        // POST: api/fundo
        [HttpPost]
        public IActionResult Post([FromBody] Fundo value)
        {
            try
            {
                _fundoService.Add(value);

                return CreatedAtAction(
                    nameof(GetByCodigo),
                    new { codigo = value.Codigo },
                    new
                    {
                        success = true,
                        message = "Fundo cadastrado com sucesso.",
                        data = value
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/fundo/FND1001
        [HttpPut("{codigo}")]
        public IActionResult Put(string codigo, [FromBody] Fundo value)
        {
            try
            {
                _fundoService.Update(codigo, value);

                return Ok(new
                {
                    success = true,
                    message = "Fundo atualizado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/fundo/FND1001
        [HttpDelete("{codigo}")]
        public IActionResult Delete(string codigo)
        {
            try
            {
                _fundoService.Delete(codigo);

                return Ok(new
                {
                    success = true,
                    message = "Fundo removido com sucesso."
                });
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/fundo/FND1001/patrimonio
        [HttpPut("{codigo}/patrimonio")]
        public IActionResult UpdatePatrimonio(
            string codigo,
            [FromBody] decimal value)
        {
            try
            {
                _fundoService.UpdatePatrimonio(codigo, value);

                return Ok(new
                {
                    success = true,
                    message = "Patrimônio atualizado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}