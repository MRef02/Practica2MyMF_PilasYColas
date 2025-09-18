// Stack ijfijefwjefjjoedsoéj hello millie i love you
using System;
using System.Collections.Generic;

public class Stack<T>
{
    private T[] items;
    private int top; // índice del elemento superior (-1 cuando está vacío y 0 cuando tiene un elemento algoa asi )

    public Stack(int capacidadInicial = 4)
    {
        if (capacidadInicial <= 0) capacidadInicial = 4;
        items = new T[capacidadInicial];
        top = -1;
    }

    public int Count => top + 1; // número de elementos en la pila se pone asi porque top empieza en -1

    public bool isvacio => top == -1; // true si la pila está vacía

    public void Push(T item)
    {
        if (top + 1 == items.Length)
            Resize(items.Length * 2);
        items[++top] = item;
    }

    public T Pop() // quita y devuelve el elemento superior
    {
        if (isvacio)
            throw new InvalidOperationException("La pila está vacía.");
        T item = items[top];
        items[top] = default(T); // liberar referencia    checaaaaar 
        top--;
        // opcional: reducir tamaño si hay mucho espacio libre (no obligatorio) mejorar la eficiencia 
        return item;
    }

    // NUEVO: Devuelve el elemento en la cima sin quitarlo
    public T Peek()
    {
        if (isvacio)
            throw new InvalidOperationException("La pila está vacía.");
        return items[top];
    }

    // NUEVO: Permite enumerar la pila desde la cima hacia la base
    public IEnumerable<T> EnumeraFromTopo()
    {
        for (int i = top; i >= 0; i--)
            yield return items[i];
    }

    private void Resize(int nuevoTam)//  esto cambia el tamaño del array interno cuano se llene pipi
    {
        T[] nuevo = new T[nuevoTam];
        Array.Copy(items, nuevo, items.Length);
        items = nuevo;
    }

    // Método  para mostrar la pila (desde cima hacia la base)
    public void ShowStack()
    {
        if (isvacio)
        {
            Console.WriteLine(" b eeeep esta  vacío.");
            return;
        }

        Console.WriteLine("Pila (tope -> base):");
        for (int i = top; i >= 0; i--)
            Console.WriteLine(items[i]?.ToString()); // el ? es para evitar excepciones si el item es null  ocupo checar otra manera de hacerlo
    }


    

}

