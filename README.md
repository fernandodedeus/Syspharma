# 💊 Syspharma - Sistema ERP Farmacêutico

O **Syspharma** é um sistema completo de gestão de ERP (Enterprise Resource Planning) voltado especificamente para drogarias e farmácias. Ele visa automatizar a operação comercial e administrativa de estabelecimentos farmacêuticos, englobando controle de vendas/NFC-e, gestão de lotes e validades de medicamentos, catálogo de produtos, gerenciamento de estoque, clientes, fornecedores e controle de acesso com autenticação segura.

---

## 🚀 Tecnologias Utilizadas

### **Backend**
* **Linguagem & Framework:** .NET 9 (`Microsoft.NET.Sdk.Web`) / C#
* **ORM:** Entity Framework Core 9.0 com Pomelo MySQL Connector (`Pomelo.EntityFrameworkCore.MySql`)
* **Banco de Dados:** MySQL
* **Autenticação e Segurança:** JWT (JSON Web Tokens), BCrypt.Net-Next para hashing de senhas
* **Documentação de API:** Swagger / OpenAPI
* **Logging:** Serilog com suporte a console e log estruturado

### **Frontend**
* **Framework:** Vue.js 3 (Composition API / Script Setup)
* **Gerenciamento de Estado:** Pinia
* **Roteamento:** Vue Router 4 / 5
* **Cliente HTTP:** Axios
* **Tooling / Bundler:** Vite com suporte a SSL / HTTPS (mkcert / basic-ssl)
* **Estilização:** CSS Vanilla modularizado e limpo com suporte a responsividade e modo escuro

---

## 🏛️ Arquitetura e Principais Módulos

### **Backend (`backend/SyspharmaApi`)**
* **`Controllers`**: Endpoints RESTful protegidos por autenticação para gerenciar:
  * **`AuthController`**: Autenticação, login e alteração de credenciais.
  * **`ProductController` & `ProductBatchController`**: Gerenciamento de catálogo de produtos, controle de validade e lotes de medicamentos.
  * **`InventoryController` & `InventoryMovementController`**: Movimentação de estoque (entradas/saídas).
  * **`OrderController` & `OrderItemController`**: Processamento de vendas e emissão/resumo de NFC-e.
  * **`UserController`**: Gestão de funcionários e cargos/permissões.
  * **`CustomerController` & `SupplierController`**: Cadastro e manutenção de clientes e fornecedores.
  * **`StoreController` & `PaymentController`**: Configurações da loja e formas de pagamento.
* **`Auth` & `Helpers`**: Middleware customizado de validação de tokens JWT, manipulador global de exceções (`GlobalExceptionHandler`) e utilitários de hash de senha.

### **Frontend (`frontend/syspharma-web`)**
* **`pages`**:
  * **Dashboard (`DashboardPage.vue`)**: Visão geral de métricas de vendas, alertas de produtos perto do vencimento e acesso rápido.
  * **Produtos (`ProdutosPage.vue`)**: Cadastro e listagem de produtos com tabela interativa.
  * **Validades (`ValidadesPage.vue`)**: Controle rigoroso de lotes e datas de expiração de medicamentos.
  * **Funcionários (`FuncionariosPage.vue`)**: Gestão de usuários e permissões do sistema.
  * **Perfil & Senha (`EditarPerfilPage.vue`, `TrocarSenhaPage.vue`)**: Configurações de conta e atualização de credenciais do usuário logado.
  * **Login (`LoginPage.vue`)**: Autenticação de acesso com armazenamento de token e sessão via Pinia.
* **`components`**: Componentes reutilizáveis como `NfceItensForm.vue`, `NfceResumo.vue`, `LotesTable.vue`, `ProdutosTable.vue`, `ConfirmModal.vue`, `UserMenu.vue` e `AppSidebar.vue`.

---

## 📄 Licença

Este projeto é de uso restrito / proprietário para gestão farmacêutica Syspharma.
