using DevExpress.Persistent.Validation;
using SCT.Module.BusinessObjects;
using System;
using System.Security.Cryptography;

namespace SCT.Module.Validations
{
    [CodeRule]
    public class PassagemRule : RuleBase<Passagem>
    {
        protected override bool IsValidInternal(
           Passagem target, out string errorMessageTemplate)
        {
            errorMessageTemplate = null;
            if (target.Itinerario != null)
            {
                if (target.Assento < 1 || target.Assento > target.Itinerario.DiaTransporte.Assentos)
                {
                    errorMessageTemplate = $"O número do assento deve estar entre 1 e {target.Itinerario.DiaTransporte.Assentos}!";
                    return false;
                }
            }
            return true;
        }
        public PassagemRule() : base("", "Save") { }
        public PassagemRule(IRuleBaseProperties properties) : base(properties) { }
    }
}
