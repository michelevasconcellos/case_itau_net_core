using System.Collections.Generic;
using CaseItau.API.Application.DTOs;
using CaseItau.API.Domain.Entities;

namespace CaseItau.API.Repositories
{
    public interface IFundoRepository
    {

        IEnumerable<FundoResponse> GetAll();
        FundoResponse GetByCodigo(string codigo);

        bool ExistsByCodigo(string codigo);
        bool ExistsByCnpj(string cnpj);

        void Add(Fundo fundo);
        void Update(string codigo, Fundo fundo);
        void Delete(string codigo);
        void UpdatePatrimonio(string codigo, decimal valor);
    }
}
