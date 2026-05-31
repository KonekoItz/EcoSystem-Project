# EcoSystem-Project
# EcoSystem Connect - Fase 1

Este repositorio contiene la infraestructura base y los cimientos de datos para el proyecto **EcoSystem Connect**, desarrollado como parte de la asignatura Programación 3.

## Arquitectura de Software: N-Capas

El sistema se ha diseñado bajo el patrón de arquitectura N-Capas (N-Tier Architecture), dividiendo las responsabilidades del software en componentes horizontales independientes y aislados:

1. **EcoSystem.API (Capa de Presentación / Servicios):** - Desarrollada en ASP.NET Core Web API.
   - Su único propósito es exponer los endpoints REST (GET, POST, PUT, DELETE) y gestionar la comunicación externa transformando las peticiones y respuestas en formato JSON estandarizado.
   - Cuenta con soporte integrado para documentación interactiva a través de Swagger.

2. **EcoSystem.Data (Capa de Acceso a Datos / Persistencia):**
   - Implementada como una biblioteca de clases (Class Library).
   - Contiene el modelo de dominio conceptual (clases POCO como `Producto` y `Categoria`) y gestiona de manera exclusiva la comunicación con el almacenamiento físico de la información.
   - Utiliza Entity Framework Core como ORM (Object-Relational Mapper) y PostgreSQL (Supabase) bajo el enfoque Code-First.

## Comunicación
La comunicación entre componentes fluye de forma estrictamente ordenada y unidireccional descendente. Cada capa habla exclusivamente con la capa inmediatamente inferior (Presentación -> Datos), garantizando el desacoplamiento total, la mantenibilidad y la escalabilidad del ecosistema digital.