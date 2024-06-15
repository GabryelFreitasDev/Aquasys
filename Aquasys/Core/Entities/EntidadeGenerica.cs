using System;

namespace CRM.Entidades.entidades
{
    public class EntidadeGenerica
    {
        public object Chave { get; set; }
        public object Valor { get; set; }

        public string ChaveString { get; set; }
        public string ValorString { get; set; }

        public int ChaveInt { get; set; }
        public int ValorInt { get; set; }

        public long ChaveLong { get; set; }
        public long ValorLong { get; set; }

        public DateTime ValorDateTime { get; set; }

        public override string ToString()
        {
            return ValorString != null && ValorString != string.Empty ? ValorString : Valor.ToString();
        }

        public EntidadeGenerica() { }

        public EntidadeGenerica(object Chave, object Valor)
        {
            this.Chave = Chave;
            this.Valor = Valor;
        }

        public bool IsChecked { get; set; } = false;
    }

    public class EntidadeGenericaCascade
    {
        public string ValorString { get; set; }
        public decimal ChaveInt { get; set; }
    }
}
