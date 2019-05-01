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
            // check if the instance of the form already exists
            // make it singleton, one instance at a time
            //QuickWinForm form = new QuickWinForm();
            QuickWinForm.getInstance();

            //form.ShowDialog();

            // This is to make the user form non-modal
            // User can work with outlook, even thought the form is open.
            //form.Show();
        }
    }
}
