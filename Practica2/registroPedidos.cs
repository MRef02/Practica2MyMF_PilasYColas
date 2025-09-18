public class Pedido
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public Pedido() { }
    public Pedido(int productoId, int cantidad)
    {
        ProductoId = productoId;
        Cantidad = cantidad;
    }
}

public class Lote
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public Lote() { }
    public Lote(int productoId, int cantidad)
    {
        ProductoId = productoId;
        Cantidad = cantidad;
    }
}
