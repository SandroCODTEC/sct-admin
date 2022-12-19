using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SCT.Module.BusinessObjects
{
    [DefaultClassOptions, NavigationItem("Eventos"), XafDisplayName("Documentos"), Persistent("Documentos")]
    public class Documento : RichTextMailMergeData
    {
        public Documento(Session session) : base(session)
        { }

    }
}
