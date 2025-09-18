﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    const string ARCHIVO = "inventario.json";

    static ListaProductos<Productos> inventario = new ListaProductos<Productos>(10);
    static Queue<Pedido> pedidos = new Queue<Pedido>(20);
    static Stack<Lote> lotes = new Stack<Lote>(20);

    static void Main()
    {
        CargarDatos();

        while (true)
        {
            Console.WriteLine("\n====== MENÚ INVENTARIO ======");
            Console.WriteLine("1) Registrar nuevo pedido (Queue)");
            Console.WriteLine("2) Procesar próximo pedido (Queue)");
            Console.WriteLine("3) Recibir mercancía (Stack)");
            Console.WriteLine("4) Reabastecer inventario (Stack)");
            Console.WriteLine("5) Ver inventario");
            Console.WriteLine("6) Ver siguiente pedido (Peek Queue)");
            Console.WriteLine("7) Ver lote en cima (Peek Stack)");
            Console.WriteLine("8) Agregar/Actualizar producto");
            Console.WriteLine("0) Salir");
            Console.Write("Opción: ");
            string op = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (op)
                {
                    case "1":
                        RegistrarPedido();
                        break;
                    case "2":
                        ProcesarPedido();
                        break;
                    case "3":
                        RecibirLote();
                        break;
                    case "4":
                        ReabastecerInventario();
                        break;
                    case "5":
                        inventario.ShowProductos();
                        break;
                    case "6":
                        VerSiguientePedido();
                        break;
                    case "7":
                        VerLoteEnCima();
                        break;
                    case "8":
                        AgregarActualizarProducto();
                        break;
                    case "0":
                        GuardarDatos();
                        Console.WriteLine("Adiós.");
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            GuardarDatos();
        }
    }

    static void RegistrarPedido()
    {
        Console.Write("ID de producto: ");
        if (!int.TryParse(Console.ReadLine(), out int pid))
        {
            Console.WriteLine("ID inválido.");
            return;
        }
        var producto = inventario.GetProductoByID(pid);
        if (producto == null)
        {
            Console.WriteLine("Producto no existe.");
            return;
        }
        Console.Write("Cantidad: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("Cantidad inválida.");
            return;
        }
        pedidos.Enqueue(new Pedido(pid, qty));
        Console.WriteLine("Pedido registrado.");
    }

    static void ProcesarPedido()
    {
        if (pedidos.Count == 0)
        {
            Console.WriteLine("No hay pedidos en cola.");
            return;
        }
        var pedido = pedidos.Dequeue();
        var producto = inventario.GetProductoByID(pedido.ProductId);
        if (producto == null)
        {
            Console.WriteLine("Producto no existe.");
            return;
        }
        if (producto.Cantidad >= pedido.Cantidad)
        {
            producto.Cantidad -= pedido.Cantidad;
            Console.WriteLine("Pedido procesado: " + pedido);
            Console.WriteLine("Stock restante de " + producto.Nombre + ": " + producto.Cantidad);
        }
        else
        {
            Console.WriteLine("Stock insuficiente. El pedido NO se procesó.");
        }
    }

    static void RecibirLote()
    {
        Console.Write("ID de producto: ");
        if (!int.TryParse(Console.ReadLine(), out int pid))
        {
            Console.WriteLine("ID inválido.");
            return;
        }
        var producto = inventario.GetProductoByID(pid);
        if (producto == null)
        {
            Console.WriteLine("Producto no existe.");
            return;
        }
        Console.Write("Unidades recibidas en el lote: ");
        if (!int.TryParse(Console.ReadLine(), out int units) || units <= 0)
        {
            Console.WriteLine("Cantidad inválida.");
            return;
        }
        lotes.Push(new Lote(pid, units));
        Console.WriteLine("Lote recibido y apilado.");
    }

    static void ReabastecerInventario()
    {
        if (lotes.Count == 0)
        {
            Console.WriteLine("No hay lotes en la pila.");
            return;
        }
        var lote = lotes.Pop();
        var producto = inventario.GetProductoByID(lote.ProductId);
        if (producto == null)
        {
            Console.WriteLine("Producto no existe.");
            return;
        }
        producto.Cantidad += lote.Unidades;
        Console.WriteLine($"Inventario reabastecido con el lote: {lote}");
        Console.WriteLine($"Nuevo stock de {producto.Nombre}: {producto.Cantidad}");
    }

    static void VerSiguientePedido()
    {
        if (pedidos.Count == 0)
            Console.WriteLine("No hay pedidos.");
        else
            Console.WriteLine("Siguiente en cola: " + pedidos.Peek());
    }

    static void VerLoteEnCima()
    {
        if (lotes.Count == 0)
            Console.WriteLine("No hay lotes.");
        else
            Console.WriteLine("Lote en cima: " + lotes.Peek());
    }

    static void AgregarActualizarProducto()
    {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Precio: ");
        if (!int.TryParse(Console.ReadLine(), out int precio))
        {
            Console.WriteLine("Precio inválido.");
            return;
        }
        Console.Write("Stock: ");
        if (!int.TryParse(Console.ReadLine(), out int stock))
        {
            Console.WriteLine("Stock inválido.");
            return;
        }
        var existente = inventario.GetProductoByID(id);
        if (existente != null)
        {
            ((Productos)existente).Cantidad = stock;
            Console.WriteLine("Producto actualizado.");
        }
        else
        {
            inventario.AddProducto(new Productos(nombre, id, precio, stock));
            Console.WriteLine("Producto agregado.");
        }
    }

    static void GuardarDatos()
    {
        var lista = new List<Productos>();
        for (int i = 0; i < inventario.Count; i++)
        {
            var prod = inventario[i];
            if (prod != null)
                lista.Add(prod);
        }
        File.WriteAllText("Producto.json", JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true }));
    }

    static void CargarDatos()
    {
        if (File.Exists("Producto.json"))
        {
            var json = File.ReadAllText("Producto.json");
            var productos = JsonSerializer.Deserialize<List<Productos>>(json);
            if (productos != null)
            {
                inventario.EliminarTodos();
                foreach (var prod in productos)
                    inventario.AddProducto(prod);
            }
        }
    }
}

public class InventarioSnapshot
{
    public List<Productos> ListaProductos { get; set; }
    public List<Pedido> PedidosEnFifo { get; set; }
    public List<Lote> LotesDesdeFondo { get; set; }
}