# Case de engenharia Itau - .Net

## Introdução
Neste projeto esta sendo utilizada a base de dados sqlite (arquivo dbcaseitau S3db) com as seguintes tabelas:

    Tabela: TIPO_FUNDO > "Tipos de fundos existentes"
	- CODIGO      - INT         NOT NULL - PRIMARY KEY
	- NOME        - VARCHAR(20) NOT NULL

    Tabela: FUNDO > "Registro relacionados ao cadastro de fundos"
	- CODIGO      - VARCHAR(20)  UNIQUE NOT NULL - PRIMARY KEY
	- NOME        - VARCHAR(100)        NOT NULL
	- CNPJ        - VARCHAR(14)  UNIQUE NOT NULL
	- CODIGO_TIPO - INT                 NOT NULL - FOREIGN KEY TIPO_FUNDO(CODIGO)
	- PATRIMONIO  - NUMERIC                 NULL

> Obs.: você pode fazer o uso do [sqliteadmin] para gerenciar a base de dados, visualizar as tabelas e seus respectivos dados

No projeto CaseItau.API foi disponibilizada uma API de Fundos com os metodos abaixo realizando acoes diretas na base de dados:

	GET                        - LISTAR TODOS OS FUNDOS CADASTRADOS
	GET    {CODIGO}            - RETORNAR OS DETALHES DE UM DETERMINADO FUNDO PELO CÓDIGO
	POST   {FUNDO}             - REALIZA O CADASTRO DE UM NOVO FUNDO
	PUT    {CODIGO}            - EDITA O CADASTRO DE UM FUNDO JÁ EXISTENTE
	DELETE {CODIGO}            - EXCLUI O CADASTRO DE UM FUNDO
	PUT    {CODIGO}/patrimonio - ADICIONA OU SUBTRAI DETERMINADO VALOR DO PATRIMONIO DE UM FUNDO

## Ações a serem realizadas
1. Faça o fork do projeto no seu github. Não realize commits na branch main e nem crie novas branchs.
2. O código da api de fundos faz mal uso dos objetos, não segue boas práticas e não possui qualidade. Refatore o codigo utilizando as melhores bibliotecas, praticas, patterns e garanta a qualidade da aplicação. Fique a vontade para usar outros componentes e base de dados.
2. Após a inclusão de um novo fundo via API, os metodos GET da API de Fundos estão retornando erro. Identifique e corrija o erro
3. Crie uma aplicação web (Angular ou ASP NET MVC) que consuma todos os metodos da API de fundos

Se a sua vaga for BACKEND não se preocupe em fazer a parte de FRONT.<br>
Após finalizar o case, envie o link do seu github com a solução final para o gestor que o solicitou.

[sqliteadmin]: <http://sqliteadmin.orbmu2k.de> 

# Solução implementada

## Refatorações realizadas

O projeto original foi refatorado com foco em boas práticas de desenvolvimento, organização de código, manutenibilidade e maior qualidade arquitetural.

Principais melhorias implementadas:

- Refatoração completa da API de Fundos
- Implementação de Repository Pattern
- Implementção de Service Layer
- Separação entre Entity e DTO
- Remoção de SQL direto da Controller
- Tratamento global de exceções com Middleware
- Logging com ILogger
- Validações de regras de negócio
- Prevenção de duplicidade de Código e CNPJ
- Validação de patrimônio negativo
- Configuração de Swagger
- Configuração de CORS
- Melhoria na organização de pastas e namespaces

---

## Problemas corrigidos

Durante o desenvolvimento foram identificados e corrigidos os seguintes problemas:

### 1. Erro no GET após inclusão de novo fundo

O método GET apresentava erro:

Input string was not in a correct format

Isso acontecia devido ao tratamento incorreto do campo PATRIMONIO, quando o valor retornava NULL.

Foi corrigido com tratamento seguro para valores nulos.

### 2. Erros de integridade no POST

Ao cadastrar novos fundos ocorriam falhas de constraint:

- UNIQUE constraint failed: FUNDO.CNPJ
- UNIQUE constraint failed: FUNDO.CODIGO

Foi implementada validação prévia para impedir duplicidade de:

- Código
- CNPJ

com retorno adequado via API.

---

### 3. SQL Injection por concatenação de string

A API original utilizava concatenação direta de SQL, o que gerava risco de falhas e vulnerabilidades.

Isso foi refatorado para uso de parâmetros SQL (`Parameters.AddWithValue()`), melhorando segurança e estabilidade.

---

### 4. database is locked

Ocorria bloqueio do SQLite devido ao mau gerenciamento de conexão.

Foi corrigido com uso adequado de `using`, garantindo fechamento correto das conexões e evitando travamentos.

---

### 5. SQL logic error

Ocorreram falhas como:

- near "Tipo": syntax error
- near "NULL": syntax error
- database schema has changed

Esses problemas foram corrigidos com:

- ajuste de queries SQL
- correção de parâmetros
- remoção de campos indevidos no body
- padronização das operações de update

---

### 6. Erro no PUT de patrimônio

O endpoint:

PUT /api/fundo/{codigo}/patrimonio

## Como executar o projeto

- Após iniciar a aplicação, acesse no navegador:
https://localhost:{porta}/swagger