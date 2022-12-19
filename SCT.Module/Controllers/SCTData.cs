using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class SCTData
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public ECongregacao Congregacao { get; set; }
        public List<ESaida> Saidas { get; set; }
        public EEvento Evento { get; set; }
        public List<EPassageiro> Passageiros { get; set; }
        public List<EDependente> Dependentes { get; set; }
        public List<EPassagem> Passagens { get; set; }
    }
}
