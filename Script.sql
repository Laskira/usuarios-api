CREATE DATABASE Colegios;

USE Colegios;

CREATE TABLE Usuarios(
    Id int IDENTITY(1, 1) primary key,
    Nombres varchar(200) not null,
    Apellidos varchar(200) not null,
    FechaNacimiento date not null,
    Celular varchar(11) not null,
    RolId int not null,
    FechaIngreso date not null,
    Email varchar(400) not null,
    Password varchar(8) not null,
    EstadoId int not null
);

ALTER TABLE
    Usuarios
ADD
    CONSTRAINT Email UNIQUE (Email);

CREATE TABLE Rol(
    Id int IDENTITY(1, 1) primary key,
    Nombre varchar(30) not null,
);

CREATE TABLE Estado(
    Id int IDENTITY(1, 1) primary key,
    Nombre varchar(30) not null,
);

/*Roles de usuarios*/
ALTER TABLE
    Usuarios
add
    foreign key (RolId) references Rol(Id);

INSERT INTO
    Rol(Nombre)
values
    ('Estudiante');

INSERT INTO
    Rol(Nombre)
values
    ('Profesor');

INSERT INTO
    Rol(Nombre)
values
    ('Directivo');

/*Estado de usuarios*/
ALTER TABLE
    Usuarios
add
    foreign key (EstadoId) references Estado(Id);

INSERT INTO
    Estado(Nombre)
values
    ('Activo');

INSERT INTO
    Estado(Nombre)
values
    ('Inactivo');

--PROCEDIMIENTOS ALMACENADOS--
--Crear usuarios
USE Colegios;

CREATE PROCEDURE SP_CrearUsuario(
    @Nombres varchar(200),
    @Apellidos varchar(200),
    @FechaNacimiento date,
    @Celular varchar(11),
    @RolId int,
    @FechaIngreso date,
    @Email varchar(400),
    @Password varchar(8),
    @EstadoId int = 1
) AS BEGIN
INSERT INTO
    Usuarios (
        Nombres,
        Apellidos,
        FechaNacimiento,
        Celular,
        RolId,
        FechaIngreso,
        Email,
        Password,
        EstadoId
    )
VALUES
    (
        @Nombres,
        @Apellidos,
        @FechaNacimiento,
        @Celular,
        @RolId,
        @FechaIngreso,
        @Email,
        @Password,
        @EstadoId
    )
END 

--Usuarios activos

CREATE PROCEDURE SP_ConsultarUsuariosActivos AS BEGIN
SELECT
    Usuarios.Id,
    Usuarios.Nombres,
    Usuarios.Apellidos,
    Usuarios.FechaNacimiento,
    Usuarios.Celular,
    Usuarios.RolId,
    Usuarios.FechaIngreso,
    Usuarios.EstadoId,
    Usuarios.Email
FROM
    Usuarios
WHERE
    EstadoId = 1;

END 

--Usuarios consultados por nombre
CREATE PROCEDURE SP_ConsultarUsuariosPorNombre(@nombre varchar(200)) AS BEGIN
SELECT
    Usuarios.Id,
    Usuarios.Nombres,
    Usuarios.Apellidos,
    Usuarios.FechaNacimiento,
    Usuarios.Celular,
    Usuarios.RolId,
    Usuarios.FechaIngreso,
    Usuarios.EstadoId,
    Usuarios.Email
FROM
    Usuarios
WHERE
    Nombres LIKE '%' + @nombre + '%';

END 

--Usuarios consultados por rol
CREATE PROCEDURE SP_ConsultarUsuariosPorRol(@rol int) AS BEGIN
SELECT
    Usuarios.Id,
    Usuarios.Nombres,
    Usuarios.Apellidos,
    Usuarios.FechaNacimiento,
    Usuarios.Celular,
    Usuarios.RolId,
    Usuarios.FechaIngreso,
    Usuarios.EstadoId,
    Usuarios.Email
FROM
    Usuarios
WHERE
    RolId = @rol;

END 

--Listar usuarios
CREATE PROCEDURE SP_ConsultarUsuarios AS BEGIN
SELECT
    Usuarios.Id,
    Usuarios.Nombres,
    Usuarios.Apellidos,
    Usuarios.FechaNacimiento,
    Usuarios.Celular,
    Usuarios.RolId,
    Usuarios.FechaIngreso,
    Usuarios.EstadoId,
    Usuarios.Email,
    Usuarios.Password
FROM
    Usuarios
END 

--Eliminar usuario
CREATE PROCEDURE SP_EliminarUsuario(@id int) AS BEGIN
DELETE FROM
    Usuarios
WHERE
    Id = @id;

END 

--Editar usuario
CREATE PROCEDURE SP_EditarUsuario(
    @Id int,
    @Nombres varchar(200),
    @Apellidos varchar(200),
    @FechaNacimiento date,
    @Celular varchar(11),
    @RolId int,
    @Password varchar(8),
    @Email varchar(400),
    @EstadoId int
) AS BEGIN
UPDATE
    Usuarios
SET
    Nombres = @Nombres,
    Apellidos = @Apellidos,
    FechaNacimiento = FechaNacimiento,
    Celular = @Celular,
    RolId = @RolId,
    Password = @Password,
    Email = @Email,
    EstadoId = @EstadoId
WHERE
    Id = @Id;

END

--Inserci�n de datos de prueba
EXEC SP_CrearUsuario 'Andres',
'Lopez',
'2000-05-17',
'3172527689',
2,
'2023-05-17',
'andres@gmail.com',
'123',
2;

EXEC SP_CrearUsuario 'Luisa',
'Ramirez',
'2000-05-17',
'3172327689',
2,
'2023-05-17',
'luisa@gmail.com',
'123';

EXEC SP_CrearUsuario 'Felipe',
'Lopez',
'2000-05-17',
'3172527689',
2,
'2023-05-17',
'felope@gmail.com',
'123';

EXEC SP_CrearUsuario 'Efrain',
'Gomez',
'2000-05-17',
'3172527689',
3,
'2023-05-17',
'Efrai@gmail.com',
'123';