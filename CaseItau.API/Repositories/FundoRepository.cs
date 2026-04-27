using System;
using System.Collections.Generic;
using System.Data.SQLite;
using CaseItau.API.Application.DTOs;
using CaseItau.API.Domain.Entities;

namespace CaseItau.API.Repositories
{
    public class FundoRepository : IFundoRepository
    {
        //private readonly string _connectionString =
        //    "Data Source=dbCaseItau.s3db";

        private readonly string _connectionString =
    @"Data Source=C:\Projeto\Case-Itau-Michele\case_itau_net_core\CaseItau.API\dbCaseItau.s3db";

        public IEnumerable<FundoResponse> GetAll()
        {
            var lista = new List<FundoResponse>();

            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    CODIGO,
                    NOME,
                    CNPJ,
                    CODIGO_TIPO,
                    PATRIMONIO
                FROM FUNDO";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new FundoResponse
                {
                    Codigo = reader["CODIGO"].ToString(),
                    Nome = reader["NOME"].ToString(),
                    Cnpj = reader["CNPJ"].ToString(),
                    CodigoTipo = int.Parse(reader["CODIGO_TIPO"].ToString()),
                    Patrimonio = reader["PATRIMONIO"] == DBNull.Value
                        ? null
                        : (decimal?)decimal.Parse(reader["PATRIMONIO"].ToString())
                });
            }

            return lista;
        }

        public bool ExistsByCodigo(string codigo)
        {
            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(1)
                FROM FUNDO
                WHERE CODIGO = @codigo";

            cmd.Parameters.AddWithValue("@codigo", codigo);

            return (long)cmd.ExecuteScalar() > 0;
        }

        public bool ExistsByCnpj(string cnpj)
        {
            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(1)
                FROM FUNDO
                WHERE CNPJ = @cnpj";

            cmd.Parameters.AddWithValue("@cnpj", cnpj);

            return (long)cmd.ExecuteScalar() > 0;
        }

        public void Add(Fundo fundo)
        {

            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO FUNDO
                (
                    CODIGO,
                    NOME,
                    CNPJ,
                    CODIGO_TIPO,
                    PATRIMONIO
                )
                VALUES
                (
                    @codigo,
                    @nome,
                    @cnpj,
                    @tipo,
                    @pat
                )";

            cmd.Parameters.AddWithValue("@codigo", fundo.Codigo);
            cmd.Parameters.AddWithValue("@nome", fundo.Nome);
            cmd.Parameters.AddWithValue("@cnpj", fundo.Cnpj);
            cmd.Parameters.AddWithValue("@tipo", fundo.CodigoTipo);
            cmd.Parameters.AddWithValue("@pat", fundo.Patrimonio);

            cmd.ExecuteNonQuery();
        }

        public void Update(string codigo, Fundo fundo)
        {
            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                    UPDATE FUNDO
                    SET
                        NOME = @nome,
                        CNPJ = @cnpj,
                        CODIGO_TIPO = @tipo,
                        PATRIMONIO = @pat
                    WHERE
                        CODIGO = @codigo";

            cmd.Parameters.AddWithValue("@nome", fundo.Nome);
            cmd.Parameters.AddWithValue("@cnpj", fundo.Cnpj);
            cmd.Parameters.AddWithValue("@tipo", fundo.CodigoTipo);
            cmd.Parameters.AddWithValue("@pat", fundo.Patrimonio);
            cmd.Parameters.AddWithValue("@codigo", codigo);

            cmd.ExecuteNonQuery();
        }

        public void Delete(string codigo)
        {
            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM FUNDO
                WHERE CODIGO = @codigo";

            cmd.Parameters.AddWithValue("@codigo", codigo);

            cmd.ExecuteNonQuery();

        }

        public void UpdatePatrimonio(string codigo, decimal valor)
        {
            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            decimal patrimonioAtual = 0;

            // busca patrimônio atual
            using (var checkCmd = con.CreateCommand())
            {
                checkCmd.CommandText = @"
                    SELECT PATRIMONIO
                    FROM FUNDO
                    WHERE CODIGO = @codigo";

                checkCmd.Parameters.AddWithValue("@codigo", codigo);

                var result = checkCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    patrimonioAtual = Convert.ToDecimal(result);
                }
            }

            var novoPatrimonio = patrimonioAtual + valor;

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE FUNDO
                    SET PATRIMONIO = @pat
                    WHERE CODIGO = @codigo";

                cmd.Parameters.AddWithValue("@pat", novoPatrimonio);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                cmd.ExecuteNonQuery();
            }

        }

        public FundoResponse GetByCodigo(string codigo)
        {
            using var con = new SQLiteConnection(_connectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    F.CODIGO,
                    F.NOME,
                    F.CNPJ,
                    F.CODIGO_TIPO,
                    F.PATRIMONIO,
                    T.NOME AS NOME_TIPO
                FROM FUNDO F
                INNER JOIN TIPO_FUNDO T
                    ON T.CODIGO = F.CODIGO_TIPO
                WHERE F.CODIGO = @codigo";

            cmd.Parameters.AddWithValue("@codigo", codigo);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new FundoResponse
                {
                    Codigo = reader["CODIGO"].ToString(),
                    Nome = reader["NOME"].ToString(),
                    Cnpj = reader["CNPJ"].ToString(),
                    CodigoTipo = Convert.ToInt32(reader["CODIGO_TIPO"]),
                    NomeTipo = reader["NOME_TIPO"].ToString(),
                    Patrimonio = reader["PATRIMONIO"] != DBNull.Value
                        ? Convert.ToDecimal(reader["PATRIMONIO"])
                        : 0
                };
            }

            return null;
        }
    }
}
