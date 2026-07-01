# API ROUTES

## AUTH

api/v1/Auth/register
POST

REQUEST:
{
    int? Idstore,
    string Name,
    string Email,
    string Password,
    string? Phone,
    string? Document
}

RESPONSE:
{
    string AcessToken,
    string RefreshToken,
    int ExpiresIn,
    int UserId,
    string FullName
}

api/v1/Auth/login
POST

REQUEST:
{
    string Email,
    string Password,
}

RESPONSE:
{
    string AcessToken,
    string RefreshToken,
    int ExpiresIn,
    int UserId,
    string FullName
}

api/v1/Auth/refresh
POST

REQUEST:
{
    string RefreshToken
}

RESPONSE:
{
    string AcessToken,
    string RefreshToken,
    int ExpiresIn,
    int UserId,
    string FullName
}

api/v1/Auth/logout
POST

REQUEST:
{
    string RefreshToken
}

api/v1/Auth/switchpass
[NEED ACCESS TOKEN]
POST

REQUEST:
{
    int Iduser,
    string Oldpass,
    string Newpass
}

RESPONSE:
{
    string AcessToken,
    string RefreshToken,
    int ExpiresIn,
    int UserId,
    string FullName
}

api/v1/Auth/me
GET

RESPONSE:
{
   int Id,
   int? Idstore,
   string FullName,
   string Email,
   string? Phone,
   string? Document,
   bool Active,
   DateTime CreatedAt
}

## CUSTOMER

api/v1/Customer
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int Idcustomer,
      string? Name,
      string? Othername,
      string Document,
      string? Phone,
      DateTime Createdat,
      bool? Active
   }
]

api/v1/Customer/{id}
GET

RESPONSE:
{
   int Idcustomer,
   string? Name,
   string? Othername,
   string Document,
   string? Phone,
   DateTime Createdat,
   bool? Active
}

api/v1/Customer
POST

REQUEST:
{
   string? Name,
   string? Othername,
   string Document,
   string? Phone,
   DateTime Createdat,
   bool? Active
}

RESPONSE:
{
   int Idcustomer,
   string? Name,
   string? Othername,
   string Document,
   string? Phone,
   DateTime Createdat,
   bool? Active
}

api/v1/Customer/{id}
PUT

REQUEST:
{
   int Idcustomer,
   string? Name,
   string? Othername,
   string Document,
   string? Phone,
   DateTime Createdat,
   bool? Active
}

RESPONSE:
204 No Content

api/v1/Customer/{id}
DELETE

RESPONSE:
204 No Content

## INVENTORY

api/v1/Inventory
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int Idinventory,
      int Idproduct,
      int Idstore,
      decimal Quantity,
      decimal Minimum
   }
]

api/v1/Inventory/{id}
GET

RESPONSE:
{
   int Idinventory,
   int Idproduct,
   int Idstore,
   decimal Quantity,
   decimal Minimum
}

api/v1/Inventory
POST

REQUEST:
{
   int Idproduct,
   int Idstore,
   decimal Quantity,
   decimal Minimum
}

RESPONSE:
{
   int Idinventory,
   int Idproduct,
   int Idstore,
   decimal Quantity,
   decimal Minimum
}

api/v1/Inventory/{id}
PUT

REQUEST:
{
   int Idinventory,
   int Idproduct,
   int Idstore,
   decimal Quantity,
   decimal Minimum
}

RESPONSE:
204 No Content

api/v1/Inventory/{id}
DELETE

RESPONSE:
204 No Content

## INVENTORYMOVEMENT

api/v1/InventoryMovement
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int IdinventoryMovement,
      int Idinventory,
      decimal Quantity,
      bool Isentry,
      decimal Balance,
      DateTime Datemov
   }
]

api/v1/InventoryMovement/{id}
GET

RESPONSE:
{
   int IdinventoryMovement,
   int Idinventory,
   decimal Quantity,
   bool Isentry,
   decimal Balance,
   DateTime Datemov
}

api/v1/InventoryMovement
POST

REQUEST:
{
   int Idinventory,
   decimal Quantity,
   bool Isentry,
   decimal Balance,
   DateTime Datemov
}

RESPONSE:
{
   int IdinventoryMovement,
   int Idinventory,
   decimal Quantity,
   bool Isentry,
   decimal Balance,
   DateTime Datemov
}

api/v1/InventoryMovement/{id}
PUT

REQUEST:
{
   int IdinventoryMovement,
   int Idinventory,
   decimal Quantity,
   bool Isentry,
   decimal Balance,
   DateTime Datemov
}

RESPONSE:
204 No Content

api/v1/InventoryMovement/{id}
DELETE

RESPONSE:
204 No Content

## ORDER

api/v1/Order
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int Idorder,
      int Idstore,
      int? Idcustomer,
      int Iduser,
      string Ordernumber,
      DateTime Orderdate,
      decimal Subtotal,
      decimal Discountvalue,
      decimal Totalvalue,
      int? Status,
      string? Notes,
      DateTime Createdat
   }
]

api/v1/Order/{id}
GET

RESPONSE:
{
   int Idorder,
   int Idstore,
   int? Idcustomer,
   int Iduser,
   string Ordernumber,
   DateTime Orderdate,
   decimal Subtotal,
   decimal Discountvalue,
   decimal Totalvalue,
   int? Status,
   string? Notes,
   DateTime Createdat
}

api/v1/Order
POST

REQUEST:
{
   int Idstore,
   int? Idcustomer,
   int Iduser,
   string Ordernumber,
   DateTime Orderdate,
   decimal Subtotal,
   decimal Discountvalue,
   decimal Totalvalue,
   int? Status,
   string? Notes,
   DateTime Createdat
}

RESPONSE:
{
   int Idorder,
   int Idstore,
   int? Idcustomer,
   int Iduser,
   string Ordernumber,
   DateTime Orderdate,
   decimal Subtotal,
   decimal Discountvalue,
   decimal Totalvalue,
   int? Status,
   string? Notes,
   DateTime Createdat
}

api/v1/Order/{id}
PUT

REQUEST:
{
   int Idorder,
   int Idstore,
   int? Idcustomer,
   int Iduser,
   string Ordernumber,
   DateTime Orderdate,
   decimal Subtotal,
   decimal Discountvalue,
   decimal Totalvalue,
   int? Status,
   string? Notes,
   DateTime Createdat
}

RESPONSE:
204 No Content

api/v1/Order/{id}
DELETE

RESPONSE:
204 No Content

## ORDERITEM

api/v1/OrderItem
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int Idorderitem,
      int Idorder,
      int Idproduct,
      int? Idbatch,
      decimal Quantity,
      decimal Unitprice,
      decimal Unitcost,
      decimal Discountvalue,
      decimal Totalvalue,
      string? Notes
   }
]

api/v1/OrderItem/{id}
GET

RESPONSE:
{
   int Idorderitem,
   int Idorder,
   int Idproduct,
   int? Idbatch,
   decimal Quantity,
   decimal Unitprice,
   decimal Unitcost,
   decimal Discountvalue,
   decimal Totalvalue,
   string? Notes
}

api/v1/OrderItem
POST

REQUEST:
{
   int Idorder,
   int Idproduct,
   int? Idbatch,
   decimal Quantity,
   decimal Unitprice,
   decimal Unitcost,
   decimal Discountvalue,
   decimal Totalvalue,
   string? Notes
}

RESPONSE:
{
   int Idorderitem,
   int Idorder,
   int Idproduct,
   int? Idbatch,
   decimal Quantity,
   decimal Unitprice,
   decimal Unitcost,
   decimal Discountvalue,
   decimal Totalvalue,
   string? Notes
}

api/v1/OrderItem/{id}
PUT

REQUEST:
{
   int Idorderitem,
   int Idorder,
   int Idproduct,
   int? Idbatch,
   decimal Quantity,
   decimal Unitprice,
   decimal Unitcost,
   decimal Discountvalue,
   decimal Totalvalue,
   string? Notes
}

RESPONSE:
204 No Content

api/v1/OrderItem/{id}
DELETE

RESPONSE:
204 No Content

## PAYMENT

api/v1/Payment
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int Idpayment,
      int Idorder,
      int Paymentmethod,
      decimal Amount,
      DateTime Paymentdate,
      int Installments,
      string? Authorizationcode,
      string? Cardlastdigits,
      string? Notes,
      DateTime Createdat
   }
]

api/v1/Payment/{id}
GET

RESPONSE:
{
   int Idpayment,
   int Idorder,
   int Paymentmethod,
   decimal Amount,
   DateTime Paymentdate,
   int Installments,
   string? Authorizationcode,
   string? Cardlastdigits,
   string? Notes,
   DateTime Createdat
}

api/v1/Payment
POST

REQUEST:
{
   int Idorder,
   int Paymentmethod,
   decimal Amount,
   DateTime Paymentdate,
   int Installments,
   string? Authorizationcode,
   string? Cardlastdigits,
   string? Notes,
   DateTime Createdat
}

RESPONSE:
{
   int Idpayment,
   int Idorder,
   int Paymentmethod,
   decimal Amount,
   DateTime Paymentdate,
   int Installments,
   string? Authorizationcode,
   string? Cardlastdigits,
   string? Notes,
   DateTime Createdat
}

api/v1/Payment/{id}
PUT

REQUEST:
{
   int Idpayment,
   int Idorder,
   int Paymentmethod,
   decimal Amount,
   DateTime Paymentdate,
   int Installments,
   string? Authorizationcode,
   string? Cardlastdigits,
   string? Notes,
   DateTime Createdat
}

RESPONSE:
204 No Content

api/v1/Payment/{id}
DELETE

RESPONSE:
204 No Content

## PRODUCTBATCH

api/v1/ProductBatch
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

RESPONSE:
[
   {
      int Idbatch,
      int Idproduct,
      int? Idsupplier,
      string Batchcode,
      DateOnly Expirationdate,
      DateOnly? Manufacturingdate,
      decimal Quantity,
      decimal Unitcost,
      DateTime Createdat
   }
]

api/v1/ProductBatch/expiringbatches
GET

RESPONSE:
[
    {
        int IdProduct,
        int IdBatch,
        string ProductInternalCode,
        string? Description,
        string BatchCode,
        int ExpiresInDays
    }
]

api/v1/ProductBatch/{id}
GET

RESPONSE:
{
   int Idbatch,
   int Idproduct,
   int? Idsupplier,
   string Batchcode,
   DateOnly Expirationdate,
   DateOnly? Manufacturingdate,
   decimal Quantity,
   decimal Unitcost,
   DateTime Createdat
}

api/v1/ProductBatch
POST

REQUEST:
{
   int Idproduct,
   int? Idsupplier,
   string Batchcode,
   DateOnly Expirationdate,
   DateOnly? Manufacturingdate,
   decimal Quantity,
   decimal Unitcost,
   DateTime Createdat
}

RESPONSE:
{
   int Idbatch,
   int Idproduct,
   int? Idsupplier,
   string Batchcode,
   DateOnly Expirationdate,
   DateOnly? Manufacturingdate,
   decimal Quantity,
   decimal Unitcost,
   DateTime Createdat
}

api/v1/ProductBatch/{id}
PUT

REQUEST:
{
   int Idbatch,
   int Idproduct,
   int? Idsupplier,
   string Batchcode,
   DateOnly Expirationdate,
   DateOnly? Manufacturingdate,
   decimal Quantity,
   decimal Unitcost,
   DateTime Createdat
}

RESPONSE:
204 No Content

api/v1/ProductBatch/{id}
DELETE

RESPONSE:
204 No Content

## PRODUCT

api/v1/Product
GET

RESPONSE:
[
   {
      int Idproduct,
      string? Description,
      string Internalcode,
      decimal Price,
      decimal Cost,
      DateTime Createdat
   }
]

api/v1/Product/{id}
GET

RESPONSE:
{
   int Idproduct,
   string? Description,
   string Internalcode,
   decimal Price,
   decimal Cost,
   DateTime Createdat
}

api/v1/Product
POST

REQUEST:
{
   string? Description,
   string Internalcode,
   decimal Price,
   decimal Cost,
   DateTime Createdat
}

RESPONSE:
{
   int Idproduct,
   string? Description,
   string Internalcode,
   decimal Price,
   decimal Cost,
   DateTime Createdat
}

api/v1/Product/{id}
PUT

REQUEST:
{
   int Idproduct,
   string? Description,
   string Internalcode,
   decimal Price,
   decimal Cost,
   DateTime Createdat
}

RESPONSE:
204 No Content

api/v1/Product/{id}
DELETE

RESPONSE:
204 No Content

## STORE

api/v1/Store
GET

RESPONSE:
[
   {
      int Idstore,
      string? Fantasyname,
      string Socialname,
      string? Cnpj,
      string? Email,
      string? Phone,
      bool? Active
   }
]

api/v1/Store/{id}
GET

RESPONSE:
{
   int Idstore,
   string? Fantasyname,
   string Socialname,
   string? Cnpj,
   string? Email,
   string? Phone,
   bool? Active
}

api/v1/Store
POST

REQUEST:
{
   string? Fantasyname,
   string Socialname,
   string? Cnpj,
   string? Email,
   string? Phone,
   bool? Active
}

RESPONSE:
{
   int Idstore,
   string? Fantasyname,
   string Socialname,
   string? Cnpj,
   string? Email,
   string? Phone,
   bool? Active
}

api/v1/Store/{id}
PUT

REQUEST:
{
   int Idstore,
   string? Fantasyname,
   string Socialname,
   string? Cnpj,
   string? Email,
   string? Phone,
   bool? Active
}

RESPONSE:
204 No Content

api/v1/Store/{id}
DELETE

RESPONSE:
204 No Content

## SUPPLIER

api/v1/Supplier
GET

RESPONSE:
[
   {
      int Idsupplier,
      string Name,
      string? Tradename,
      string? Document,
      string? Email,
      string? Phone,
      bool? Active,
      DateTime Createdat
   }
]

api/v1/Supplier/{id}
GET

RESPONSE:
{
   int Idsupplier,
   string Name,
   string? Tradename,
   string? Document,
   string? Email,
   string? Phone,
   bool? Active,
   DateTime Createdat
}

api/v1/Supplier
POST

REQUEST:
{
   string Name,
   string? Tradename,
   string? Document,
   string? Email,
   string? Phone,
   bool? Active,
   DateTime Createdat
}

RESPONSE:
{
   int Idsupplier,
   string Name,
   string? Tradename,
   string? Document,
   string? Email,
   string? Phone,
   bool? Active,
   DateTime Createdat
}

api/v1/Supplier/{id}
PUT

REQUEST:
{
   int Idsupplier,
   string Name,
   string? Tradename,
   string? Document,
   string? Email,
   string? Phone,
   bool? Active,
   DateTime Createdat
}

RESPONSE:
204 No Content

api/v1/Supplier/{id}
DELETE

RESPONSE:
204 No Content

## USER

api/v1/User
GET

Optional Params:
page: int (default = 0)
pagesize: int (default = 10)

### IF ROLE = ADMIN
RESPONSE:
[
   {
    int Iduser,
    int? Idstore, 
    int Role,
    string Name,
    string Email,
    string? Cpf,
    string? Phone, 
    bool Active,
    DateTime Createdat,
    string? Profilephotopath 
   }
]

### IF ROLE != ADMIN
RESPONSE:
[
   {
    int Iduser,
    string Name,
    string? ProfilePhotoPath,
    bool Active
   }
]

api/v1/User/{id}
GET

RESPONSE:
{
   int Iduser,
   int? Idstore,
   string Name,
   string Email,
   string? Cpf,
   string Pass,
   string? Phone,
   bool Active,
   DateTime Createdat
}

api/v1/User
POST

REQUEST:
{
   int? Idstore,
   string Name,
   string Email,
   string? Cpf,
   string Pass,
   string? Phone,
   bool Active,
   DateTime Createdat
}

RESPONSE:
{
   int Iduser,
   int? Idstore,
   string Name,
   string Email,
   string? Cpf,
   string Pass,
   string? Phone,
   bool Active,
   DateTime Createdat
}

api/v1/User/{id}
PUT

REQUEST:
{
   int Iduser,
   int? Idstore,
   string Name,
   string Email,
   string? Cpf,
   string Pass,
   string? Phone,
   bool Active,
   DateTime Createdat
}

RESPONSE:
204 No Content

api/v1/User/{id}
DELETE

RESPONSE:
204 No Content
