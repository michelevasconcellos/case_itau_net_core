using System.Collections.Generic;
using CaseItau.API.Application.DTOs;
using CaseItau.API.Domain.Entities;

namespace CaseItau.API.Services
{
    public interface IFundoService
    {
        IEnumerable<FundoResponse> GetAll();
        FundoResponse GetByCodigo(string codigo);

        void Add(Fundo fundo);
        void Update(string codigo, Fundo fundo);
        void Delete(string codigo);
        void UpdatePatrimonio(string codigo, decimal valor);
    }
}