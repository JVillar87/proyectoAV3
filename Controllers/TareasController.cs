using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using TodoMVC.Data;
using TodoMVC.Models;

namespace TodoMVC.Controllers;

public class PeliculasySeriesController : Controller
{
    public IActionResult Index()
    {
        var peliculasyseries = new List<PeliculasySeries>();

        var conexion = Database.AbrirConexion();
        var sql = "SELECT Id, Titulo, Genero, Estreno, Completada FROM PeliculasySeries ORDER BY Id DESC";
        var comando = new SqliteCommand(sql, conexion);
        var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            peliculasyseries.Add(new PeliculasySeries
            {
                Id = reader.GetInt32(0),
                Titulo = reader.GetString(1),
                Genero = reader.GetString(2),
                Estreno = reader.GetDateTime(3),
                Completada = reader.GetBoolean(4)
            });
        }

        return View(peliculasyseries);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(PeliculasySeries peliseries)
    {
        if (string.IsNullOrWhiteSpace(peliseries.Genero))
        {
            ViewData["Error"] = "El género es obligatorio.";
            return View(peliseries);
        }

        if (string.IsNullOrWhiteSpace(peliseries.Titulo) || peliseries.Titulo.Trim().Length < 2)
        {
            ViewData["Error"] = "El título es obligatorio y debe tener mínimo 2 caracteres.";
            return View(peliseries);
        }

        var conexion = Database.AbrirConexion();
        var sql = "INSERT INTO PeliculasySeries (Titulo, Genero, Estreno, Completada) VALUES (@titulo, @genero, @estreno, @completada)";
        var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@titulo", peliseries.Titulo);
        comando.Parameters.AddWithValue("@genero", peliseries.Genero);
        comando.Parameters.AddWithValue("@estreno", peliseries.Estreno);
        comando.Parameters.AddWithValue("@completada", peliseries.Completada ? 1 : 0);
        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }

    public IActionResult Editar(int id)
    {
        var conexion = Database.AbrirConexion();
        var sql = "SELECT Id, Titulo, Genero, Estreno, Completada FROM PeliculasySeries WHERE Id = @id";
        var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        var reader = comando.ExecuteReader();

        if (reader.Read())
        {
            var peliseries = new PeliculasySeries
            {
                Id = reader.GetInt32(0),
                Titulo = reader.GetString(1),
                Genero = reader.GetString(2),
                Estreno = reader.GetDateTime(3),
                Completada = reader.GetInt32(4) == 1
            };
            return View(peliseries);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Editar(PeliculasySeries peliseries)
    {

        if (string.IsNullOrWhiteSpace(peliseries.Titulo) || peliseries.Titulo.Trim().Length < 2)
        {
            ViewData["Error"] = "El título es obligatorio y debe tener mínimo 2 caracteres.";
            return View(peliseries);
        }

        //Valida género
        if (string.IsNullOrWhiteSpace(peliseries.Genero))
        {
            ViewData["Error"] = "El género es obligatorio.";
            return View(peliseries);
        }

        using var conexion = Database.AbrirConexion();
        var sql = "UPDATE PeliculasySeries SET Titulo = @titulo, Genero = @genero, Estreno = @estreno, Completada = @completada WHERE Id = @id";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@titulo", peliseries.Titulo.Trim());
        comando.Parameters.AddWithValue("@genero", peliseries.Genero.Trim());
        comando.Parameters.AddWithValue("@estreno", peliseries.Estreno);
        comando.Parameters.AddWithValue("@completada", peliseries.Completada ? 1 : 0);
        comando.Parameters.AddWithValue("@id", peliseries.Id);

        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }



    public IActionResult Completar(int id)
    {
        var conexion = Database.AbrirConexion();
        var sql = "UPDATE PeliculasySeries SET Completada = 1 WHERE Id = @id";
        var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        var conexion = Database.AbrirConexion();
        var sql = "DELETE FROM PeliculasySeries WHERE Id = @id";
        var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        comando.ExecuteNonQuery();

        return RedirectToAction("Index");
    }
}



/*

PUNTO DE PARTIDA
────────────────
Este proyecto sigue la estructura clásica
de tres capas (Modelo / Vista / Controlador) y realiza las operaciones CRUD
básicas: listar, crear, completar y eliminar tareas.
 
Vuestra misión es entender ese código base, cambiar el contexto de la
aplicación y añadir las mejoras que se describen a continuación.
 
 

PARTE 1 · CAMBIO DE CONTEXTO  (obligatorio)


  A) Biblioteca personal
     Entidad: Libro  (titulo, autor, leido: bool)
 
  B) Gestor de películas / series
     Entidad: Pelicula  (titulo, genero, vista: bool)
 
  C) Lista de la compra
     Entidad: Producto  (nombre, cantidad, comprado: bool)
 
  D) Agenda de contactos
     Entidad: Contacto  (nombre, telefono, email)
 
  E) Registro de entrenamientos
     Entidad: Entreno  (fecha, tipo, duracion_minutos, completado: bool)
 
La elección es libre, pero hay que justificarla en la memoria (ver Parte 4).
 
 

PARTE 2 · MEJORAS OBLIGATORIAS

 
2.1  AÑADIR UN CAMPO EXTRA AL MODELO
     El modelo original solo tiene Id, Titulo y Completada.
     Añade al menos un campo adicional relevante para tu contexto.
     Ejemplos: fecha de creación, prioridad (alta/media/baja), categoría...
 
     Pasos que implica:
     · Actualizar la clase del modelo (Models/)
     · Modificar el CREATE TABLE en Database.cs
     · Actualizar las queries SQL en el controlador
     · Mostrar el nuevo campo en la vista Index
 
2.2  EDITAR UN REGISTRO
     La aplicación base no permite editar una entrada ya creada.
     Implementa la funcionalidad de edición:
 
     El formulario de edición debe pre-rellenar los campos con los valores
     actuales del registro.
 
2.3  VALIDACIÓN EN EL CONTROLADOR
     Antes de insertar o actualizar, comprueba que los campos obligatorios
     no están vacíos. Si la validación falla, devuelve el formulario con un
     mensaje de error visible al usuario (sin redirigir).
 
     Mínimo: validar que el campo principal (nombre, título…) no está vacío
     y que tiene más de 2 caracteres. (puedes utilizar regex en el fromulario)
 
 
PARTE 3 · MEJORA OPCIONAL (puntuación extra)

 
Implementa UNA de las siguientes mejoras adicionales:
 
  · FILTRADO: Añade un filtro en la vista Index (pendientes / completados / todos)
              pasando el filtro como parámetro en la URL (?filtro=pendientes).

  · DETALLE:    Añade una vista de detalle (GET /Entidad/Detalle/{id}) que
                muestre toda la información de un registro en una página propia.
 
 

PARTE 4 · DOCUMENTACIÓN

Genera un archivo de readme con la siguiente información:
 
  1. Contexto elegido y justificación (3-5 líneas)
  2. Diagrama o descripción de la estructura de la BD (tabla, columnas, tipos)
  3. URLs de la aplicación y qué acción realiza cada una
  4. Explicación de los cambios realizados respecto al código base
  5. Capturas de pantalla de todas la vistas funcionando y estiladas con CSS en plan guapo.
  

  */