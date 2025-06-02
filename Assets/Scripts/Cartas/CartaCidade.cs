// Carta de Cidade
public class CartaCidade : ICarta
{
    public string Cidade { get; }
    public string Tipo => "Cidade";
    public string Cor { get; }

    public CartaCidade(string cidade, string cor)
    {
        Cidade = cidade;
        Cor = cor;
    }
}