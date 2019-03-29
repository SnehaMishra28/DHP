using log4net;
using Microsoft.Office.Tools.Ribbon;

namespace QuickWinsSpOutlookAddIn
{
    public partial class Ribbon1
    {
        private static ILog log = LogManager.GetLogger(typeof(Ribbon1));

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        // This is the click event to the Quick Wins button in the 
        // Ribbon1 that opens the Quick Wins form
        private void btnForm_Click(object sender, RibbonControlEventArgs e)
        {
            log.Info("Inside btnForm_Click - to open form!");
            QuickWinForm form = new QuickWinForm();
            form.ShowDialog();
        }
    }
}
