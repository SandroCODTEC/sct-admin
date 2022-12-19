using DevExpress.Persistent.Validation;
using SCT.Module.BusinessObjects;
using System;

namespace SCT.Module.Validations
{
    [CodeRule]
    public class ItinerarioRule : RuleBase<Itinerario>
    {
        protected override bool IsValidInternal(
           Itinerario target, out string errorMessageTemplate)
        {
            errorMessageTemplate = null;

            var totalAssentos = target.DiaTransporte.Assentos;
            var totalPassagens = target.DiaTransporte.Itinerarios.Sum(c => c.Passagens.Count);
            if (totalPassagens > totalAssentos)
            {
                errorMessageTemplate = $"Você possui {totalAssentos} assentos, mas está tentando inserir {totalPassagens} passageiros!";
                return false;
            }
            return true;
        }
        public ItinerarioRule() : base("", "Save") { }
        public ItinerarioRule(IRuleBaseProperties properties) : base(properties) { }
    }
}
