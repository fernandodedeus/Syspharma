# 💊 Syspharma Web - ERP Farmacêutico

Sistema completo de Gestão e Planejamento de Recursos Empresariais (**ERP - Enterprise Resource Planning**) desenvolvido especificamente para **drogarias e farmácias**. O Syspharma automatiza toda a operação comercial e administrativa farmacêutica, incluindo controle rigoroso de lotes e validades de medicamentos, catálogo de produtos, gestão de estoque, vendas/NFC-e e controle de acessos de funcionários.

---

## 🚀 Tecnologias Utilizadas

### **Frontend (`syspharma-web`)**
* **Framework:** [Vue 3](https://vuejs.org/) (Composition API com `<script setup>`)
* **Bundler & Tooling:** [Vite 8](https://vitejs.dev/) com suporte a SSL/HTTPS local (`vite-plugin-mkcert` / `@vitejs/plugin-basic-ssl`)
* **Gerenciamento de Estado:** [Pinia 3](https://pinia.vuejs.org/) (Autenticação, tokens e estado global do usuário)
* **Roteamento:** [Vue Router 5](https://router.vuejs.org/) com Navigation Guards e proteção de rotas
* **Cliente HTTP:** [Axios](https://axios-http.com/) com interceptadores globais para anexar token JWT
* **Estilização:** CSS Vanilla modularizado, design responsivo, componentes customizados e suporte a temas.

### **Backend (`SyspharmaApi`)**
* **Framework:** [.NET 9 Web API](https://dotnet.microsoft.com/) C#
* **ORM & Banco de Dados:** Entity Framework Core 9 com connector [Pomelo MySQL](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
* **Autenticação & Segurança:** JWT (*JSON Web Tokens*) com validação estrita + hashing de senhas com `BCrypt.Net-Next`
* **Documentação de API:** Swagger / OpenAPI
* **Logging:** Serilog (Console e Logs estruturados)

---

## 📦 Módulos e Funcionalidades do ERP

### 📊 **1. Dashboard Operacional**
* Visão geral de métricas de vendas e produtos em tempo real.
* Painel de alertas preditivos para medicamentos próximos ao vencimento.
* Acessos rápidos às ações mais utilizadas do sistema.

### 💊 **2. Catálogo de Produtos e Medicamentos**
* Cadastro detalhado de medicamentos e produtos farmacêuticos.
* Associação com fornecedores, categoria, código de barras e precificação.

### ⏳ **3. Controle de Lotes e Validades**
* Rastreabilidade por número de lote e data de expiração.
* Indicadores visuais de status (*Válido*, *Próximo do Vencimento*, *Vencido*).
* Ações de prevenção contra perda de estoque por vencimento.

### 📄 **4. Emissão e Gestão de Notas Fiscais (NFC-e / NF-e)**
* Painel lateral de caixa para emissão rápida de NFC-e (Nota Fiscal de Consumidor Eletrônica).
* Inclusão dinâmica de itens, cálculo automático de desconto e totalização.
* Vinculação opcional de CPF do consumidor e escolha da forma de pagamento.

### 👥 **5. Gestão de Funcionários e Permissões**
* Cadastro de usuários com papéis diferenciados (*Administrador*, *Farmacêutico*, *Operador de Caixa*).
* Controle de acesso baseado em roles (RBAC) protegendo rotas no frontend e endpoints na API.

---

## 📂 Estrutura do Projeto

```text
Syspharma/
├── backend/
│   └── SyspharmaApi/              # API Web .NET 9 (C#)
│       ├── Auth/                  # Serviços e handlers de autenticação JWT
│       ├── Context/               # DbContext do Entity Framework Core
│       ├── Controllers/           # Endpoints RESTful (Auth, Product, Batch, Order, User, etc.)
│       ├── Helpers/               # GlobalExceptionHandler, PasswordHasher
│       ├── Models/                # Entidades do domínio (Product, ProductBatch, Order, User, etc.)
│       └── appsettings.json       # Configurações do backend (Conexão MySQL, JWT)
│
└── frontend/
    └── syspharma-web/             # Aplicação Single Page Application (Vue 3 + Vite)
        ├── public/                # Logos, favicons e recursos estáticos
        └── src/
            ├── api/               # Módulos de integração HTTP (axios, endpoints)
            ├── components/        # Componentes reutilizáveis (Formulários, Tabelas, Modais, Sidebar)
            ├── pages/             # Telas da aplicação (Dashboard, Produtos, Validades, NF-e, Perfil)
            ├── router/            # Configuração do Vue Router e rotas protegidas
            ├── stores/            # Stores do Pinia (authStore)
            └── main.js            # Inicialização da aplicação Vue 3
```

---

## 🛠️ Como Executar o Projeto Localmente

### **Pré-requisitos**
* [Node.js](https://nodejs.org/) (versão 18 ou superior)
* [SDK .NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
* Banco de Dados **MySQL** (local ou em nuvem)

---

### **1. Configuração do Backend (.NET API)**

1. Navegue até a pasta do backend:
   ```bash
   cd backend/SyspharmaApi
   ```

2. Crie ou ajuste o arquivo `appsettings.Development.json` (ou `appsettings.json`) com as suas credenciais locais. **Importante:** Nunca versione senhas ou chaves reais em repositórios públicos.

   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "Jwt": {
       "Issuer": "Syspharma",
       "Audience": "Syspharma.Clients",
       "Key": "SUA_CHAVE_SECRETA_JWT_AQUI_MINIMO_32_CARACTERES",
       "ExpirationMinutes": 60
     },
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;port=3306;database=syspharma;user id=root;password=SUA_SENHA_LOCAL"
     },
     "AllowedHosts": "*"
   }
   ```

3. Compile e execute a API:
   ```bash
   dotnet restore
   dotnet run
   ```
   A API iniciará por padrão em `http://localhost:5253` (ou porta definida em `launchSettings.json`). A documentação interativa **Swagger** estará disponível em `http://localhost:5253/swagger`.

---

### **2. Configuração do Frontend (Vue 3)**

1. Navegue até a pasta do frontend:
   ```bash
   cd frontend/syspharma-web
   ```

2. Instale as dependências:
   ```bash
   npm install
   ```

3. (Opcional) Crie um arquivo `.env.local` para configurar a URL da API se for diferente do padrão:
   ```env
   VITE_API_URL=http://localhost:5253/api/v1
   ```

4. Inicie o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```

5. Acesse a aplicação no seu navegador (geralmente em `http://localhost:5173`).

---

### **3. Gerar Build de Produção do Frontend**

Para validar ou gerar a versão de produção otimizada:
```bash
npm run build
```
Os arquivos estáticos serão gerados no diretório `dist/`.

---

## 🔒 Segurança e Boas Práticas

* **Zero Hardcoded Secrets:** Chaves de assinatura JWT e credenciais de banco de dados devem ser injetadas exclusivamente via Variáveis de Ambiente ou `appsettings.json` local (ignorado pelo `.gitignore`).
* **Proteção de Rotas:** O frontend verifica o token de autenticação em cada navegação (`router.beforeEach`).
* **Comunicação Segura:** Requisições à API utilizam cabeçalhos `Authorization: Bearer <token>` validados no backend pelo middleware personalizado.

---

## 📄 Licença

Este projeto é de uso restrito e proprietário do sistema **Syspharma ERP Farmacêutico**.
