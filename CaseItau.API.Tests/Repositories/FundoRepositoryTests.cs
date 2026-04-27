using Xunit;
using CaseItau.API.Repositories;
using CaseItau.API.Domain.Entities;

namespace CaseItau.API.Tests.Repositories
{
    public class FundoRepositoryTests
    {
        private readonly FundoRepository _repository;

        public FundoRepositoryTests()
        {
            SQLitePCL.Batteries.Init();
            _repository = new FundoRepository();
        }

        [Fact]
        public void Add_DeveCadastrarNovoFundo()
        {
            var fundo = new Fundo
            {
                Codigo = "TESTE001",
                Nome = "Fundo Teste",
                Cnpj = "99999999999999",
                CodigoTipo = 1,
                Patrimonio = 1000
            };

            if (_repository.ExistsByCodigo(fundo.Codigo))
            {
                _repository.Delete(fundo.Codigo);
            }

            _repository.Add(fundo);

            var resultado = _repository.GetByCodigo(fundo.Codigo);

            Assert.NotNull(resultado);
            Assert.Equal("TESTE001", resultado.Codigo);
        }

        [Fact]
        public void GetAll_DeveRetornarListaDeFundos()
        {
            // Act
            var resultado = _repository.GetAll();

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
        }

    }
}