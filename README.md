# Gestor de Películas y Series

Aplicación web con arquitectura MVC (Modelo - Vista - Controlador) que permite gestionar una colección personal de películas y series.
El proyecto incluye operaciones CRUD completas, edición de registros y mejoras visuales con CSS.

---

# 1. Contexto elegido y justificación

Elegí la opción de **gestor de películas / series** porque es un contexto cercano y fácil de relacionar con el uso CRUD.  
Además, permite trabajar de forma sencilla conceptos como películas/series vistas/no vistas, géneros. 
La idea era añadir poster pero no llegué a tiempo.  

---

# 2. Estructura de la Base de Datos

## Tabla: "Peliculas"

| Columna | Tipo | Descripción |
|---|---|---|
| Id | INTEGER | Identificador único |
| Titulo | TEXT | Nombre de la película o serie |
| Genero | TEXT | Género de la película |
| Vista | BOOLEAN | Indica si fue vista o no |
| FechaCreacion | TEXT | Fecha en la que se añadió |

---
## Modelo utilizado

```csharp
public class Pelicula
{
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Genero { get; set; }

    public bool Vista { get; set; }

    public string FechaCreacion { get; set; }
}
