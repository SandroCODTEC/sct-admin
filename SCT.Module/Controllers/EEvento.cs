using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class EEvento
    {
        public Guid Oid { get; set; }
        public bool Concluido { get; set; }
        public string Descricao { get; set; }
        public string Tema { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public decimal ValorPassagem { get; set; }
    }
}
