using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class EPassageiro
    {
        public Guid Oid { get; set; }
        public List<Guid> Dependentes { get; set; }
        public string Nome { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Celular { get; set; }
        public bool Ativo { get; set; }
    }
}
