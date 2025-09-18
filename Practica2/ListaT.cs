using System;
using System.Collections.Generic;

using System;
//A1_Mafer_Can 
public interface IProducto
{
    string Nombre { get; }
    int ID { get; }
    int Precio { get; }
    int Cantidad { get; set; }
}

public class Productos : IProducto
{
    public string Nombre { get; private set; }
    public int ID { get; private set; }
    public int Precio { get; private set; }
    public int Cantidad { get; set; }

    public Productos(string nombre, int id, int precio, int cantidad)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío");
        if (id < 10000 || id > 99999)
            throw new ArgumentException("El ID debe tener 5 dígitos");
        if (cantidad < 0)
            throw new ArgumentException("La cantidad no puede ser negativa");

        Nombre = nombre;
        ID = id;
        Precio = precio;
        Cantidad = cantidad;
    }

    public override string ToString()
    {
        return $"Nombre: {Nombre}, ID: {ID}, Precio: {Precio}, Cantidad: {Cantidad}";
    }
}
