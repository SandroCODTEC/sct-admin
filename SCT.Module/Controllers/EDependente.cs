using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class EDependente
    {
        public Guid Oid { get; set; }
        public string Nome { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public EPassageiro Passageiro { get; set; }
    }
}
