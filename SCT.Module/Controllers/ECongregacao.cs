using System;
using System.Linq;

namespace SCT.Module.Controllers
{
    public class ECongregacao
    {
        public Guid Oid { get; set; }
        public string Nome { get; set; }
        public string Responsavel { get; set; }
        public string EmailResponsavel { get; set; }
        public string CelularResponsavel { get; set; }
        public string Ajudante { get; set; }
        public string EmailAjudante { get; set; }
        public string CelularAjudante { get; set; }
    }
}
