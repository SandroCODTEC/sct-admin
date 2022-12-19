using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;
public class GetActionController : WindowController
{
    public GetActionController()
    {
        TargetWindowType = WindowType.Main;
    }
    protected override void OnActivated()
    {
        base.OnActivated();
        Frame.GetController<EditModelController>()?.EditModelAction.Active.SetItemValue("AlwaysActive", false);
        Frame.GetController<ConfigureSkinController>()?.ConfigureSkinAction.Active.SetItemValue("myReason", false);
        var r = Frame.GetController<ResetViewSettingsController>().ResetViewSettingsAction;
        r.Active.SetItemValue("myReason", false);
        r.Category = "Tools";

    }

}
