using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace TestOutlookAddIn
{
    public partial class ManageTaskPaneRibbon
    {
        private void ManageTaskPaneRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void toggleButton1_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TaskPane.Visible = ((RibbonToggleButton)sender).Checked;
            //Form1 frm = new Form1();

            //frm.ShowDialog();
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            Form1 frm = new Form1();

            frm.ShowDialog();
        }

        /*
         * Form1 frm = new Form1();
            frm.Close();
         * 
         * 
         */
    }
}
