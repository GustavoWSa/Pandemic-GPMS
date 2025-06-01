// Carta de infecção
public class CartaInfeccao : ICarta
{
    public string Cidade { get; }
    public string Tipo => "Infecção";
    public string Cor { get; }

    public CartaInfeccao(string cidade, string cor)
    {
        Cidade = cidade;
        Cor = cor;
    }
}