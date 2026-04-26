using System;
using System.Collections.Generic;
using CaseItau.API.Application.DTOs;
using CaseItau.API.Domain.Entities;
using CaseItau.API.Repositories;
using Microsoft.Extensions.Logging;

namespace CaseItau.API.Services
{
    public class FundoService : IFundoService
    {
        private readonly IFundoRepository _repository;
        private readonly ILogger<FundoService> _logger;

        public FundoService(
            IFundoRepository repository,
            ILogger<FundoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<FundoResponse> GetAll()
        {
            _logger.LogInformation("Consultando todos os fundos cadastrados.");

            return _repository.GetAll();
        }

        public FundoResponse GetByCodigo(string codigo)
        {
            _logger.LogInformation(
                "Consultando fundo pelo código: {Codigo}",
                codigo);

            return _repository.GetByCodigo(codigo);
        }

        public void Add(Fundo fundo)
        {
            _logger.LogInformation(
                "Iniciando cadastro do fundo: {Codigo}",
                fundo.Codigo);

            if (_repository.ExistsByCodigo(fundo.Codigo) ||
                _repository.ExistsByCnpj(fundo.Cnpj))
            {
                _logger.LogWarning(
                    "Tentativa de cadastro duplicado para o fundo: {Codigo}",
                    fundo.Codigo);

                throw new Exception(
                    "Já existe um fundo cadastrado com este código ou CNPJ.");
            }

            _repository.Add(fundo);

            _logger.LogInformation(
                "Fundo cadastrado com sucesso: {Codigo}",
                fundo.Codigo);
        }

        public void Update(string codigo, Fundo fundo)
        {
            _logger.LogInformation(
                "Iniciando atualização do fundo: {Codigo}",
                codigo);

            if (!_repository.ExistsByCodigo(codigo))
            {
                _logger.LogWarning(
                    "Tentativa de atualização de fundo inexistente: {Codigo}",
                    codigo);

                throw new Exception("Fundo não encontrado.");
            }

            var atual = _repository.GetByCodigo(codigo);

            if (atual != null &&
                atual.Cnpj != fundo.Cnpj &&
                _repository.ExistsByCnpj(fundo.Cnpj))
            {
                _logger.LogWarning(
                    "Tentativa de atualização com CNPJ duplicado para fundo: {Codigo}",
                    codigo);

                throw new Exception(
                    "Já existe outro fundo com este CNPJ.");
            }

            _repository.Update(codigo, fundo);

            _logger.LogInformation(
                "Fundo atualizado com sucesso: {Codigo}",
                codigo);
        }

        public void Delete(string codigo)
        {
            _logger.LogInformation(
                "Iniciando exclusão do fundo: {Codigo}",
                codigo);

            if (!_repository.ExistsByCodigo(codigo))
            {
                _logger.LogWarning(
                    "Tentativa de exclusão de fundo inexistente: {Codigo}",
                    codigo);

                throw new Exception("Fundo não encontrado.");
            }

            _repository.Delete(codigo);

            _logger.LogInformation(
                "Fundo removido com sucesso: {Codigo}",
                codigo);
        }

        public void UpdatePatrimonio(string codigo, decimal valor)
        {
            _logger.LogInformation(
                "Iniciando movimentação de patrimônio do fundo: {Codigo}, Valor: {Valor}",
                codigo,
                valor);

            if (!_repository.ExistsByCodigo(codigo))
            {
                _logger.LogWarning(
                    "Tentativa de movimentação em fundo inexistente: {Codigo}",
                    codigo);

                throw new Exception("Fundo não encontrado.");
            }

            _repository.UpdatePatrimonio(codigo, valor);

            _logger.LogInformation(
                "Patrimônio atualizado com sucesso para o fundo: {Codigo}",
                codigo);
        }
    }
}