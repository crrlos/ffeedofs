create table Clientes
(
    Id INT PRIMARY KEY IDENTITY,
    Codigo VARCHAR(10),
    Nombre VARCHAR(100),
    Apellidos VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
)

create table ClientesAudit
(
    Id INTEGER,
    Codigo VARCHAR(10),
    Nombre VARCHAR(100),
    Apellidos VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)
)
drop table TiposCreditos
create table TiposCreditos
(
    Id INT PRIMARY KEY IDENTITY,
    Codigo VARCHAR(10),
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
)
create table TiposCreditosAudit
(
    Id INT PRIMARY KEY IDENTITY,
    Codigo VARCHAR(10),
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)
)

create table Destinos
(
    Id int PRIMARY KEY IDENTITY,
    Codigo VARCHAR(10),
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
)
create table DestinosAudit
(
    Id int PRIMARY KEY IDENTITY,
    Codigo VARCHAR(10),
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)
)
create table Paises
(
    Id INT PRIMARY key IDENTITY,
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
)
create table PaisesAudit
(
    Id INT PRIMARY key IDENTITY,
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)
)
create table Departamentos
(
    Id INT PRIMARY key IDENTITY,
    PaisId INT,
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
        FOREIGN KEY(PaisId) REFERENCES Paises(Id)
)
create table DepartamentosAudit
(
    Id INT PRIMARY key IDENTITY,
    PaisId INT,
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)
        FOREIGN KEY(PaisId) REFERENCES Paises(Id)
)
create table Municipios
(
    Id INT PRIMARY key IDENTITY,
    DepartamentoId INT,
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
        FOREIGN KEY(DepartamentoId) REFERENCES Departamentos(Id)
)
create table MunicipiosAudit
(
    Id INT PRIMARY key IDENTITY,
    DepartamentoId INT,
    Nombre VARCHAR(100),
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)

        FOREIGN KEY(DepartamentoId) REFERENCES Departamentos(Id)
)
create table SolicitudCredito
(
    Id INT PRIMARY key IDENTITY,
    ClienteId INT,
    Ingresos DECIMAL(10,2),
    Egresos DECIMAL(10,2),
    MontoSolicitado DECIMAL(10,2),
    Plazo INT,
    Tasa DECIMAL(10,5),
    DestinoId INT,
    TipoCreditoId INT,
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME
        FOREIGN KEY(ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY(DestinoId) REFERENCES Destinos(Id),
    FOREIGN KEY(TipoCreditoId) REFERENCES TiposCreditos(Id),
)

create table SolicitudCreditoAudit
(
    Id INT,
    ClienteId INT,
    Ingresos DECIMAL(10,2),
    Egresos DECIMAL(10,2),
    MontoSolicitado DECIMAL(10,2),
    Plazo INT,
    Tasa DECIMAL(10,5),
    DestinoId INT,
    TipoCreditoId INT,
    UsuarioModificacion VARCHAR(100),
    FechaModificacion DATETIME,
    Accion varchar(10)
)

create table Amortizacion(
    Id int PRIMARY key IDENTITY,
    SolicitudCreditoId Int, 
    Periodo int, 
    Cuota decimal (10,2),
    Interes decimal(10,2),
    Amortizacion decimal(10,2),
    Saldo decimal(10,2)
)

GO

CREATE TRIGGER ClientesAuditT 
ON Clientes 
after INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @event_type varchar(42)
    IF EXISTS(SELECT *
    FROM inserted)
  IF EXISTS(SELECT *
    FROM deleted)
    SELECT @event_type = 'update'
ELSE
    SELECT @event_type = 'insert'
ELSE
  IF EXISTS(SELECT *
    FROM deleted)
    SELECT @event_type = 'delete'

    INSERT INTO ClientesAudit
    select *, @event_type
    from deleted
END


GO

delete from solicitudcredito
select * from Amortizacion
