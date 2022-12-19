using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class EPassagem
    {
        public Guid Oid { get; set; }
        public EEvento Evento { get; set; }
        public EPassageiro Passageiro { get; set; }
        public List<Guid> Dependentes { get; set; }
        public List<int> Dias { get; set; }
        public EGrupo Grupo { get; set; }
        public ESaida Saida { get; set; }
    }
}
