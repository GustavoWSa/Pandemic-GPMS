// Carta de Evento
public class CartaEvento : ICarta
{
    public string Nome => Tipo.ToString();
    public string Tipo => "Evento";
    public TipoEvento TipoEvento { get; }
    public string Descricao { get; }

    public CartaEvento(TipoEvento tipo)
    {
        TipoEvento = tipo;
        Descricao = ObterDescricao(tipo);
    }

    private string ObterDescricao(TipoEvento tipo)
    {
        return tipo switch
        {
            TipoEvento.PonteAerea => "Mova qualquer peão para qualquer cidade.",
            TipoEvento.ConcessaoGovernamental => "Coloque um centro de pesquisa em qualquer cidade.",
            TipoEvento.Previsao => "Veja as próximas 6 cartas de infecção e reorganize-as como quiser.",
            TipoEvento.HoraDoSilencio => "Pule a próxima fase de infecção.",
            TipoEvento.RespostaImediata => "Jogue esta carta fora do seu turno para realizar uma ação gratuita.",
            _ => "Evento desconhecido."
        };
    }
}
