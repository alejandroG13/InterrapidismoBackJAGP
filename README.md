# Backend - Sistema de Inscripciones

Este proyecto corresponde al **backend** de un sistema de inscripciones para estudiantes. Está desarrollado en **.NET 10**, utilizando **Entity Framework Core** con **MySQL** como base de datos, y JWT para autenticación.  

El backend expone **endpoints REST** que el frontend (Angular 21) consume para manejar inscripciones, estudiantes y materias.

---

## Tecnologías

- .NET 10
- Entity Framework Core 8
- MySQL
- JWT para autenticación
- Swagger para documentación de la API
- Buenas prácticas: DTOs, servicios, validaciones y separación de responsabilidades

---

## Configuración

1. Clonar el repositorio:

  git clone <URL_DEL_REPO>
  cd backend

2. Crear la base de datos siguiendo los scrips proporcionados
 - DDL_BD_UNIVERSIDAD.sql
 - DML_BD_UNIVERSIDAD.sql

 3. Restaurar paquetes NuGet:
  dotnet restore

 4. Ejecutar el proyecto:
  dotnet run


