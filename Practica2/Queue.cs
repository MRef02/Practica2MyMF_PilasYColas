using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualBasic;

public class MyQueue<Type>
{
    private Type[] _items;
    private Type[] Items
    {
        get
        {
            return _items;
        }
        set
        {
            if (value is null)
            {
                throw new Exception("La queue siempre tiene que tener una instancia de objetos");
            }
            _items = value;
        }

    }

    private readonly int _capacity;
    public int Capacity
    {
        get
        {
            return _capacity;
        }
        init
        {
            if (value <= 0)
            {
                throw new Exception("La capacidad no puede ser menor o igual a cero");
            }
            _capacity = value;
        }
    }

    private int _count;
    public int Count
    {
        get
        {
            return _count;
        }
        private set
        {
            if (value > Capacity)
            {
                throw new Exception("La cuenta de elementos no puede ser mayor a la capacidad");
            }
            else if (value < 0)
            {
                throw new Exception("La cuenta de elementos no puede ser menor a cero");
            }
            _count = value;
        }
    }

    public bool IsFull
    {
        get
        {
            return Count == Capacity;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return Count == 0;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public MyQueue(int capacidad = 20)
    {
        Capacity = capacidad;
        Items = new Type[capacidad];
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void Enqueue(Type item)
    {
        if (this.IsFull)
        {
            throw new Exception("La cola está llena");
        }

        Items[Count] = item;
        Count++;
    }

    public Type Dequeue()
    {
        if (this.IsEmpty)
        {
            throw new Exception("La cola esta vacia");
        }

        Type temp = Items[0];
        for (int i = 1; i < Count; i++)
        {
            Items[i - 1] = Items[i];
        }

        Count--;
        return temp;
    }

    // NUEVO: Devuelve el primer elemento sin quitarlo
    public Type Peek()
    {
        if (this.IsEmpty)
            throw new Exception("La cola está vacía");
        return Items[0];
    }

    // NUEVO: Permite enumerar la cola desde el inicio
    public IEnumerable<Type> EnumeraFromInicio()
    {
        for (int i = 0; i < Count; i++)
            yield return Items[i];
    }

}