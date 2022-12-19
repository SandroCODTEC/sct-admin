using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class ESaida
    {
        public Guid Oid { get; set; }
        public int Parada { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime Horario { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }
}
