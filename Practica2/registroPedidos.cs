public class Pedido
{
    public int ProductId { get; set; }
    public int Cantidad { get; set; }

    public Pedido(int productId, int cantidad)
    {
        ProductId = productId;
        Cantidad = cantidad;
    }

    public override string ToString()
    {
        return $"ProductoID: {ProductId}, Cantidad: {Cantidad}";
    }
}
public class Lote
{
    public int ProductId { get; set; }
    public int Unidades { get; set; }

    public Lote(int productId, int unidades)
    {
        ProductId = productId;
        Unidades = unidades;
    }

    public override string ToString()
    {
        return $"ProductoID: {ProductId}, Unidades: {Unidades}";
    }
}
