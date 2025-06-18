create database DBMensajeria

CREATE TABLE Rol (
    IdRol INT PRIMARY KEY,
    NombreRol NVARCHAR(100),
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME
);

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY,
    Correo NVARCHAR(150) UNIQUE,
    ContrasenaHash NVARCHAR(255),
    NombreCompleto NVARCHAR(200),
    FotoPerfil VARBINARY(MAX),
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME
);

CREATE TABLE UsuarioRol (
    IdUsuario INT,
    IdRol INT,
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME,
    PRIMARY KEY (IdUsuario, IdRol),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);

CREATE TABLE Mensaje (
    IdMensaje INT PRIMARY KEY,
    IdRemitente INT,
    IdDestinatario INT,
    Contenido NVARCHAR(MAX),
    Editado BIT,
    FechaEnvio DATETIME,
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME,
    FOREIGN KEY (IdRemitente) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdDestinatario) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE HistorialConversacion (
    IdHistorial INT PRIMARY KEY,
    IdUsuario INT,
    IdConversacionCon INT,
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdConversacionCon) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE TokenSesion (
    IdTokenSesion INT PRIMARY KEY,
    IdUsuario INT,
    Token NVARCHAR(500),
    FechaExpiracion DATETIME,
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE RecuperacionPassword (
    IdRecuperacion INT PRIMARY KEY,
    IdUsuario INT,
    TokenRecuperacion NVARCHAR(255),
    FechaExpiracion DATETIME,
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);
CREATE TABLE Contacto (
    IdContacto INT PRIMARY KEY,
    IdUsuario INT,
    IdUsuarioContacto INT,
    Alias NVARCHAR(100),
    Estado CHAR(1),
    FechaRegistro DATETIME,
    FechaActualizacion DATETIME,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdUsuarioContacto) REFERENCES Usuario(IdUsuario)
);

CREATE TABLE EstadoUsuario (
    IdEstado INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    EstaEnLinea BIT NOT NULL,
    UltimaConexion DATETIME NOT NULL,
    Estado NVARCHAR(20) NOT NULL,
    FechaRegistro DATETIME NOT NULL,
    FechaActualizacion DATETIME NOT NULL
);
