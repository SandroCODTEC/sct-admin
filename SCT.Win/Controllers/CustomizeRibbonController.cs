using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraSpellChecker;

public class CustomizeRibbonController : WindowController
{
    protected override void OnActivated()
    {
        base.OnActivated();
        UserLookAndFeel.Default.SetSkinStyle(SkinStyle.WXI);

        Window.TemplateChanged += Window_TemplateChanged;
    }
    private void Window_TemplateChanged(object sender, EventArgs e)
    {
        if (Window.Template is RibbonForm && Window.Template is ISupportStoreSettings)
        {
            ((ISupportStoreSettings)Window.Template).SettingsReloaded += OnFormReadyForCustomizations;
        }
    }
    private void OnFormReadyForCustomizations(object sender, EventArgs e)
    {
        if (((RibbonForm)sender).Ribbon != null)
            ((RibbonForm)sender).Ribbon.ItemPanelStyle = RibbonItemPanelStyle.Skin;
    }
    protected override void OnDeactivated()
    {
        Window.TemplateChanged -= Window_TemplateChanged;
        base.OnDeactivated();
    }
}
