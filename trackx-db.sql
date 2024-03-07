CREATE DATABASE DB_CF;

USE DB_CF;

CREATE TABLE TB_ROL(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	UsuarioCreacionAuditoria INT NOT NULL,
	FechaCreacionAuditoria DATETIME2(7) NOT NULL,
	UsuarioActualizacionAuditoria INT NULL,
	FechaActualizacionAuditoria DATETIME2(7) NULL,
	UsuarioEliminacionAuditoria INT NULL,
	FechaEliminacionAuditoria DATETIME2(7) NULL,
	Estado INT NOT NULL
);

CREATE TABLE TB_USUARIO(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Nombre VARCHAR(50) NOT NULL,
	Apellido VARCHAR(50) NOT NULL,
	Correo VARCHAR(100) NOT NULL,
	Pass VARCHAR(100) NOT NULL,
	Tipo VARCHAR(7) NOT NULL,
	Cliente VARCHAR(MAX) NULL,
	IdRol INT NOT NULL,
	UsuarioCreacionAuditoria INT NOT NULL,
	FechaCreacionAuditoria DATETIME2(7) NOT NULL,
	UsuarioActualizacionAuditoria INT NULL,
	FechaActualizacionAuditoria DATETIME2(7) NULL,
	UsuarioEliminacionAuditoria INT NULL,
	FechaEliminacionAuditoria DATETIME2(7) NULL,
	Estado INT NOT NULL,
	CONSTRAINT FK_ROL_USUARIO FOREIGN KEY (IdRol) REFERENCES TB_ROL(Id)
);

ALTER TABLE TB_USUARIO
ADD UsuarioCreacionAuditoria INT NOT NULL,
    FechaCreacionAuditoria DATETIME2(7) NOT NULL,
    UsuarioActualizacionAuditoria INT NULL,
    FechaActualizacionAuditoria DATETIME2(7) NULL,
    UsuarioEliminacionAuditoria INT NULL,
    FechaEliminacionAuditoria DATETIME2(7) NULL,
    Estado INT NOT NULL;