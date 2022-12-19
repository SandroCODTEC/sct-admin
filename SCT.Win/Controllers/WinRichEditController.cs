using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.XtraSpellChecker;
public partial class WinRichEditController : ViewController
{
    // ...
    protected override void OnActivated()
    {
        base.OnActivated();
        RichTextServiceController controller = Frame.GetController<RichTextServiceController>();
        if (controller != null)
        {
            controller.CustomizeRichEditControl += Controller_CustomizeRichEditControl;
        }
    }
    private void Controller_CustomizeRichEditControl(object sender, CustomizeRichEditEventArgs e)
    {
        SpellChecker spellChecker = new SpellChecker();
        spellChecker.SetSpellCheckerOptions(e.RichEditControl, new OptionsSpelling());
        spellChecker.SpellCheckMode = SpellCheckMode.AsYouType;
        e.RichEditControl.SpellChecker = spellChecker;
        e.RichEditControl.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout;
    }
}
