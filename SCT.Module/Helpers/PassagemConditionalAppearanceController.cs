using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using SCT.Module.BusinessObjects;

namespace SCT.Module.Helpers
{


    namespace MainDemo.Module.Controllers
    {
        public class PassagemConditionalAppearanceController : ViewController
        {
            private AppearanceController appearanceController;
            public PassagemConditionalAppearanceController()
            {
                TargetObjectType = typeof(Passagem);
            }
            protected override void OnActivated()
            {
                base.OnActivated();
                appearanceController = Frame.GetController<AppearanceController>();
                if (appearanceController != null)
                {
                    appearanceController.CustomApplyAppearance += appearanceController_CustomApplyAppearance;
                }
            }
            void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
            {
                if (e.ContextObjects == null || e.ContextObjects.Length != 1) return;
                Passagem obj = e.ContextObjects[0] as Passagem;
                if (obj == null) return;

                if (obj.Grupo != null)
                {
                    e.AppearanceObject.FontColor = obj.Grupo.Cor;
                }
            }
            protected override void OnDeactivated()
            {
                if (appearanceController != null)
                {
                    appearanceController.CustomApplyAppearance -= appearanceController_CustomApplyAppearance;
                }
                base.OnDeactivated();
            }
        }
    }
}
