using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static ListaProductos<Productos> inventario = new ListaProductos<Productos>(10);
    static Queue<Pedido> pedidos = new Queue<Pedido>();
    static Stack<Lote> lotes = new Stack<Lote>();

    static readonly string FILE_PATH = Path.Combine(AppContext.BaseDirectory, "Producto.json");

    static void Main(string[] args)
    {
        CargarDatos();
        int opcion;

        do
        {
            Console.WriteLine("\n--- Menú Inventario ---");
            Console.WriteLine("1. Registrar un nuevo pedido (Queue)");
            Console.WriteLine("2. Procesar el próximo pedido (Queue)");
            Console.WriteLine("3. Recibir mercancía (Stack)");
            Console.WriteLine("4. Reabastecer inventario (Stack)");
            Console.WriteLine("5. Mostrar inventario");
            Console.WriteLine("6. Mostrar pedidos pendientes");
            Console.WriteLine("7. Mostrar lotes en almacén");
            Console.WriteLine("8. Agregar o actualizar producto");
            Console.WriteLine("9. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Opción inválida.");
                continue;
            }

            switch (opcion)
            {
                case 1: RegistrarPedido(); break;
                case 2: ProcesarPedido(); break;
                case 3: RecibirMercancia(); break;
                case 4: ReabastecerInventario(); break;
                case 5: MostrarInventario(); break;
                case 6: MostrarPedidos(); break;
                case 7: MostrarLotes(); break;
                case 8: AgregarOActualizarProducto(); break;
                case 9: GuardarDatos(); Console.WriteLine("Saliendo..."); break;
                default: Console.WriteLine("Opción inválida."); break;
            }
        } while (opcion != 9);
    }

    // ---------------- MENÚ ----------------

    static void RegistrarPedido()
    {
        Console.Write("Ingrese ID del producto: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Ingrese cantidad: ");
        int cantidad = int.Parse(Console.ReadLine());

        Pedido pedido = new Pedido { ProductoId = id, Cantidad = cantidad };
        pedidos.Enqueue(pedido);
        Console.WriteLine("Pedido registrado.");
        GuardarDatos(); // <--- NUEVO: persistir después de cambios
    }

    static void ProcesarPedido()
    {
        if (pedidos.Count > 0)
        {
            Pedido pedido = pedidos.Dequeue();
            Productos producto = inventario.BuscarPorId(pedido.ProductoId);

            if (producto != null && producto.Cantidad >= pedido.Cantidad)
            {
                producto.Cantidad -= pedido.Cantidad;
                Console.WriteLine($"Pedido de {pedido.Cantidad} {producto.Nombre} procesado.");
                GuardarDatos(); // Guardar cambios en inventario
            }
            else
            {
                Console.WriteLine("No hay suficiente inventario o producto no encontrado.");
            }
        }
        else
        {
            Console.WriteLine("No hay pedidos pendientes.");
        }
    }

    static void RecibirMercancia()
    {
        Console.Write("Ingrese ID del producto recibido: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Ingrese cantidad de cajas: ");
        int cantidad = int.Parse(Console.ReadLine());

        Lote lote = new Lote { ProductoId = id, Cantidad = cantidad };
        lotes.Push(lote);
        Console.WriteLine("Lote recibido y almacenado en la pila.");
        GuardarDatos(); // <--- NUEVO: persistir después de cambios
    }


    static void ReabastecerInventario()
    {
        if (lotes.Count > 0)
        {
            Lote lote = lotes.Pop();
            Productos producto = inventario.BuscarPorId(lote.ProductoId);

            if (producto != null)
            {
                producto.Cantidad += lote.Cantidad;
            }
            else
            {
                producto = new Productos("Desconocido", lote.ProductoId, 0, lote.Cantidad);
                inventario.AddProducto(producto);
            }

            Console.WriteLine("Inventario reabastecido con el lote recibido.");
            GuardarDatos(); // Guardar cambios en inventario
        }
        else
        {
            Console.WriteLine("No hay lotes en el almacén.");
        }
    }

    static void MostrarInventario()
    {
        if (inventario.Count == 0)
        {
            Console.WriteLine("Inventario vacío.");
            return;
        }

        for (int i = 0; i < inventario.Count; i++)
        {
            var p = inventario[i];
            if (p != null)
                Console.WriteLine($"{p.Nombre} - {p.Cantidad} unidades - ${p.Precio}");
        }
    }

    static void MostrarPedidos()
    {
        if (pedidos.Count == 0)
        {
            Console.WriteLine("No hay pedidos pendientes.");
            return;
        }

        foreach (var pedido in pedidos)
            Console.WriteLine($"Producto ID: {pedido.ProductoId}, Cantidad: {pedido.Cantidad}");
    }

    static void MostrarLotes()
    {
        if (lotes.Count == 0)
        {
            Console.WriteLine("No hay lotes en almacén.");
            return;
        }

        foreach (var lote in lotes)
        {
            Console.WriteLine($"Producto ID: {lote.ProductoId}, Cantidad: {lote.Cantidad}");
        }
    }

    static void AgregarOActualizarProducto()
    {
        Console.Write("Ingrese ID del producto: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Ingrese nombre del producto: ");
        string nombre = Console.ReadLine();
        Console.Write("Ingrese precio: ");
        int precio = int.Parse(Console.ReadLine());
        Console.Write("Ingrese cantidad: ");
        int cantidad = int.Parse(Console.ReadLine());

        Productos producto = inventario.BuscarPorId(id);
        if (producto != null)
        {
            producto.Nombre = nombre;
            producto.Precio = precio;
            producto.Cantidad += cantidad;
            Console.WriteLine("Producto actualizado.");
        }
        else
        {
            producto = new Productos(nombre, id, precio, cantidad);
            inventario.AddProducto(producto);
            Console.WriteLine("Producto agregado al inventario.");
        }

        GuardarDatos(); // Guardar cambios en inventario
    }

    // ---------------- PERSISTENCIA ----------------

    static void GuardarDatos()
    {
        try
        {
            var lista = new List<Productos>();
            for (int i = 0; i < inventario.Count; i++)
            {
                var prod = inventario[i];
                if (prod != null) lista.Add(prod);
            }
            File.WriteAllText(FILE_PATH,
                JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine($"✅ Datos guardados en: {FILE_PATH}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error al guardar: " + ex.Message);
        }
    }


    static void CargarDatos()
    {
        if (File.Exists(FILE_PATH))
        {
            var json = File.ReadAllText(FILE_PATH);
            var productos = JsonSerializer.Deserialize<List<Productos>>(json);
            if (productos != null)
            {
                inventario.EliminarTodos();
                foreach (var prod in productos) inventario.AddProducto(prod);
                Console.WriteLine($" Inventario cargado desde {FILE_PATH} ({productos.Count} productos)");
            }
        }
        else
        {
            Console.WriteLine("No se encontró archivo previo, inventario vacío.");
        }
    }
}