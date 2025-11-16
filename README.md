
# 📞 ArcusTel.PortalClientes.Api – <br> API de Gerenciamento de DIDs Nacionais e Internacionais

![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=sql-server&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-6DB33F?style=for-the-badge&logo=ef&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Scalar API](https://img.shields.io/badge/Scalar-API%20Documentation-4BC51D?style=for-the-badge&logo=swagger&logoColor=white)

O repositório contém a **ArcusTel.PortalClientes.Api**, uma API ASP.NET Core responsável por gerenciar **DIDs nacionais (+55) e internacionais**, integrando-se com os parceiros **BrasilConnect** e **WorldTel**. A aplicação utiliza boas práticas de arquitetura, separação em camadas, DTOs bem definidos e persistência de dados via **Entity Framework Core** com SQL Server.

---

## 🚀 Tecnologias Utilizadas

* **.NET 10 / ASP.NET Core**
* **C# 12**
* **Entity Framework Core** + Migrations
* **SQL Server** como banco principal
* **Injeção de Dependência (DI)**
* **RESTful Controllers**
* **Autenticação JWT**
* **Docker** para deploy containerizado
* **xUnit** para testes unitários

---

## 🔑 Funcionalidades Principais

A API permite:

1. **Ativar DIDs**  
   - Nacionais (+55) via [**BrasilConnect**](https://github.com/danhpaiva/partner-brasilconnect-minimal-api-did-net)  
   - Internacionais via [**WorldTel**](https://github.com/danhpaiva/partner-worldtel-mvc-api-did-net)

2. **Consultar status de ativação de DIDs**  

3. **Registro de logs de resposta dos parceiros**  

4. **Normalização do status de DIDs** independente do parceiro (**NormalizedStatus enum**)

5. **Integração transparente com múltiplos parceiros** (**PartnerId enum**)

---

## 📡 Endpoints

### 1️⃣ Ativar DID

~~~http
POST /api/dids/activate
~~~

**Request Body**

~~~json
{
  "E164Number": "+5511999999999",
  "UserId": 123
}
~~~

**Response (exemplo)**

~~~json
{
  "ExternalId": "abc123",
  "DidNumber": "+5511999999999",
  "PartnerId": "BrasilConnect",
  "Status": "Active",
  "DetailMessage": "Ativação concluída com sucesso",
  "CreatedAt": "2025-11-16T15:00:00Z"
}
~~~

---

### 2️⃣ Consultar status de um DID

~~~http
GET /api/dids/{requestId}/status
~~~

**Response (exemplo)**

~~~json
{
  "ExternalId": "abc123",
  "DidNumber": "+5511999999999",
  "PartnerId": "BrasilConnect",
  "Status": "Active",
  "DetailMessage": "",
  "CreatedAt": "2025-11-16T15:00:00Z"
}
~~~

---

## 🗄️ Banco de Dados

O contexto principal é `AppDbContext.cs` com os seguintes **DbSets**:

* `TB_DidActivationRequest` – registros de requisições de ativação de DIDs
* `TB_PartnerResponseLog` – logs das respostas recebidas dos parceiros
* `TB_PartnerStatusMapping` – mapeamento de status normalizado
* `TB_User` – usuários do sistema

### Enum de Status (`NormalizedStatus`)

~~~csharp
public enum NormalizedStatus
{
    Pending = 0,
    Active = 1,
    Failed = 2,
    Error = 3,
    InvalidData = 4,
    Unknown = 5
}
~~~

### Enum de Parceiros (`PartnerId`)

~~~csharp
public enum PartnerId
{
    BrasilConnect,
    WorldTel
}
~~~

---

## 🧪 Testes Automatizados

Os testes estão no projeto `ArcusTel.PortalClientes.Tests` e incluem:

* Validação de criação de DIDs
* Testes de integração com parceiros (mockados)
* Consulta de status
* Validação de logs e normalização

Executar testes:

~~~bash
dotnet test
~~~

---

## 🐳 Docker

### **Build da imagem**

~~~bash
docker build -t arcustel-portalclientes-api .
~~~

### **Executar container**

~~~bash
docker run -p 8080:80 arcustel-portalclientes-api
~~~

---

## ▶️ Como Executar Localmente

1. Clone o repositório:

~~~bash
git clone https://github.com/danhpaiva/arcustel-portalclientes-solution-net/
~~~

2. Acesse a pasta do projeto:

~~~bash
cd ArcusTel.PortalClientes.Api
~~~

3. Restaure dependências:

~~~bash
dotnet restore
~~~

4. Configure a **connection string** no `appsettings.json`:

~~~json
"ConnectionStrings": {
  "AppDbContext": "Server=(localdb)\\mssqllocaldb;Database=ArcusTelPortalClientes;Trusted_Connection=True;MultipleActiveResultSets=true"
}
~~~

5. Configure os endpoints dos parceiros e credenciais:

~~~json
"Apis": {
  "PartnerBrasil": "https://localhost:7094",
  "WorldTel": "https://localhost:7264"
},
"Partners": {
  "PartnerBrasil": { "Username": "admin", "Password": "senha123" },
  "WorldTel": { "Username": "admin", "Password": "admin123" }
}
~~~

~~~
BrasilConectApi

https://github.com/danhpaiva/partner-brasilconnect-minimal-api-did-net
~~~

~~~
WorldTelApi

https://github.com/danhpaiva/partner-worldtel-mvc-api-did-net
~~~

6. Execute migrations (se necessário):

~~~bash
dotnet ef database update
~~~

7. Rode a aplicação:

~~~bash
dotnet run
~~~

A API estará disponível em:

~~~
https://localhost:7040
~~~

Para documentação interativa (Swagger/OpenAPI):

~~~
https://localhost:7040/scalar/
~~~

---

## 🤝 Integração com Parceiros

A API encapsula a comunicação com:

* **BrasilConnect** – DIDs nacionais (+55)
* **WorldTel** – DIDs internacionais

Funcionalidades:

* Ativação de DIDs
* Consulta de status
* Registro de logs normalizados

---

## 🛠️ Melhorias Futuras

* Implementar **refresh token** para parceiros
* Logging estruturado com **Serilog**
* Testes de integração completos
* Implementação de **camada de repositório** (Clean Architecture)
* Endpoint de geração automática de DIDs em lote

---

## 📄 Licença

Este projeto está sob licença **MIT**.

---

## 👨‍💻 Autor

**Daniel Paiva**  
Desenvolvedor .NET | Professor Universitário

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/danhpaiva/)
![Stars](https://img.shields.io/github/stars/danhpaiva/arcustel-portalclientes-solution-net?style=for-the-badge)
![Forks](https://img.shields.io/github/forks/danhpaiva/arcustel-portalclientes-solution-net?style=for-the-badge)
![Issues](https://img.shields.io/github/issues/danhpaiva/arcustel-portalclientes-solution-net?style=for-the-badge)
