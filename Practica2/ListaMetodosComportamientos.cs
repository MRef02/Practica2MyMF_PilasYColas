using System.Collections.Generic;
using System.Xml.Serialization;

using System;
//A1_Mafer_Can
public class ListaProductos<T> where T : IProducto
{
    private T[] productos;
    private int cantidadActual;

    public ListaProductos(int capacidadInicial = 4)
    {
        productos = new T[capacidadInicial];
        cantidadActual = 0;
    }

    public void AddProducto(T newProducto)
    {
        if (cantidadActual == productos.Length)
            Redimensionar();

        productos[cantidadActual] = newProducto;
        cantidadActual++;
    }
    public int Count
{
    get { return cantidadActual; }
}


    private void Redimensionar()
    {
        int newLength = productos.Length * 2;
        T[] nuevoArray = new T[newLength];
        for (int i = 0; i < productos.Length; i++)
            nuevoArray[i] = productos[i];
        productos = nuevoArray;
    }

    private void ShowProducto(int index)
    {
        if (index >= 0 && index < cantidadActual)
            Console.WriteLine(productos[index]);
    }

    public void ShowProductos()
    {
        for (int i = 0; i < cantidadActual; i++)
            ShowProducto(i);
    }

    public void SearchProducto(int id)
    {
        for (int i = 0; i < cantidadActual; i++)
        {
            if (productos[i].ID == id)
            {
                ShowProducto(i);
                return;
            }
        }
        Console.WriteLine("Producto no encontrado.");
    }

    public void ActualizarCantidad(int id, int nuevaCantidad)
    {
        for (int i = 0; i < cantidadActual; i++)
        {
            if (productos[i].ID == id)
            {
                productos[i].Cantidad = nuevaCantidad;
                return;
            }
        }
        throw new ArgumentException("Producto con el ID especificado no encontrado.");
    }
      public void EliminarTodos()
    {
        for (int i = 0; i < cantidadActual; i++)
            productos[i] = default(T);

        cantidadActual = 0;
    }

    public void EliminarProducto(int id)
    {
        for (int i = 0; i < cantidadActual; i++)
        {
            if (productos[i].ID == id)
            {
                for (int j = i; j < cantidadActual - 1; j++)
                    productos[j] = productos[j + 1];

                productos[cantidadActual - 1] = default(T);
                cantidadActual--;
                return;
            }
        }
        throw new ArgumentException("Producto con el ID especificado no encontrado.");
    }

    public void OrdenarPorNombre()
    {
        QuickSort(0, cantidadActual - 1);
    }

    private void QuickSort(int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(low, high);
            QuickSort(low, pi - 1);
            QuickSort(pi + 1, high);
        }
    }

    private int Partition(int low, int high)
    {
        string pivot = productos[high].Nombre;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (string.Compare(productos[j].Nombre, pivot) < 0)
            {
                i++;
                T temp = productos[i];
                productos[i] = productos[j];
                productos[j] = temp;
            }
        }

        T temp1 = productos[i + 1];
        productos[i + 1] = productos[high];
        productos[high] = temp1;

        return i + 1;
    }

    // Devuelve el producto por ID o null si no existe
    public T GetProductoByID(int id)
    {
        for (int i = 0; i < Count; i++)
            if (this[i].ID == id)
                return this[i];
        return default(T);
    }

    // NUEVO: Devuelve el producto por índice (para guardar/cargar)
    public T GetProductoByIDIndex(int index)
    {
        if (index >= 0 && index < cantidadActual)
            return productos[index];
        return default(T);
    }

    // NUEVO: Permite indexar la lista como un array (para GuardarDatos)
    public T this[int index]
    {
        get
        {
            if (index >= 0 && index < cantidadActual)
                return productos[index];
            return default(T);
        }
    }
}
