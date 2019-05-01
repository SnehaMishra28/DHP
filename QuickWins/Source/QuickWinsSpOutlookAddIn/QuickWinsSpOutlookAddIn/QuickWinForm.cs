using System;
using log4net;
using System.Windows.Forms;
using RestSharp;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Linq;

namespace QuickWinsSpOutlookAddIn
{
    public partial class QuickWinForm : Form
    {
        private static ILog log = LogManager.GetLogger(typeof(QuickWinForm));

        // Variables used for the different SP items in QuickWins List
        String requestedBy;
        String source;
        String system;
        String issue;
        String otherIssue;
        String resolution;
        String otherResolution;
        String ticketDate;
        //DateTime datePicker;
        SpItemObject spFilledItemObject = new SpItemObject();
        CreateSpListItem client;
        IRestResponse<RootObject> qwRespObj;

        // Constructor to Initialize the Form
        // Create the client object and
        // Finish rest of the form's initialization if required.
        private QuickWinForm()
        {
            log.Info("Inside QuickWinForm constructor to create the Quick Win form!");

            InitializeComponent();

            FinishInitializing();

            client = new CreateSpListItem();

            //if (client != null)
            //{
            //    qwRespObj = client.RetrieveQWOptionItems();
            //}
            // get list of System for combo from the resp obj
            // get list of problem for combo from the resp obj
            // get list of resolution for combo from the resp obj
            // get only the active components

            // pass the 3 list to initialize the list in combo box
        }

        public static QuickWinForm getInstance()
        {
            // Check if the instance of form is already available
            if (sFormInstance == null)
            { 
                //if there is no instance available... create new one
                sFormInstance = new QuickWinForm();
            }

            // This is to make the user form non-modal
            // User can work with outlook, even thought the form is open.
            sFormInstance.Show();
            sFormInstance.BringToFront();

            return sFormInstance;
        }

        //private static QuickWinForm sFormInstance = new QuickWinForm();
        private static QuickWinForm sFormInstance;

        // Function to finish the Form's Initialization, 
        // perform different logical changes to the form before it is loaded to the User
        // Returns nothing
        private void FinishInitializing()
        {
            log.Info("Inside FinishInitializing func to set the dropdown items in the Quick Win form!");

            // Hiding the Other text box for Problem and resolution, 
            //and show them only when option 'Other' is selected

            txtOherIssue.Visible = false;
            layoutControlItem11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            txtOtherResolution.Visible = false;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            InitializeVariables();

            //Getting the QW Options list items for dropdown

            //FormDropdownOptions f1 = new FormDropdownOptions();
            List<string> sList = FormDropdownOptions.systemList;
            List<string> pList = FormDropdownOptions.problemList;
            List<string> rList = FormDropdownOptions.resolutionList;

            List<SpRecords> spRecordList = FormDropdownOptions.records;

            // ------------------> comboSystem

            //this.comboSystem.Items.AddRange(new object[] {
            //"Baan",
            //"IFS",
            //"Cerp",
            //"Admin",
            //"Sharepoint",
            //"Hardware/ Software",
            //"Help Desk",
            //"Shoretel",
            //"Other"});

            //var sys = spRecordList.Distinct<SpRecords>(t=> t.system);
            var sys = spRecordList.Select(s => s.system).Distinct();

            comboSystem.Items.Clear();
            comboSystem.Items.AddRange(sys.ToArray());
            //comboSystem.Items.Add("Other");

            //sys.Count();
            //System.Object[] ItemObjectS = new System.Object[sys.Count()+1];
            //for (int i = 0; i < sys.Count(); i++)
            //{
            //    ItemObjectS[i] = sys.ElementAt(i);
            //}
            //ItemObjectS[sys.Count() ] = "Other";
            
            //comboSystem.Items.AddRange(ItemObjectS);
            //System.Object[] ItemObject1 = new System.Object[sList.Count];
            //for (int i = 0; i < sList.Count; i++)
            //{
            //    ItemObject1[i] = sList[i];
            //}

            //comboSystem.Items.AddRange(ItemObject1);
            log.Info("Set the dropdown items in the Quick Win form for System combo box!");

            // ------------------> comboIssue Problem

            //this.comboIssue.Items.AddRange(new object[] {
            //"Other",
            //"Can't Login"});

            var prob = spRecordList.Where(s => s.system == comboSystem.Text).Select(p => p.problem).Distinct();

            comboIssue.Items.Clear();
            comboIssue.Items.AddRange(prob.ToArray());
            comboIssue.Items.Add("Other");

            //System.Object[] ItemObjectP = new System.Object[prob.Count() + 1];
            //for (int i = 0; i < prob.Count(); i++)
            //{
            //    ItemObjectP[i] = prob.ElementAt(i);
            //}
            //ItemObjectP[prob.Count()] = "Other";
            //comboIssue.Items.Clear();
            //comboIssue.Items.AddRange(ItemObjectP);

            //System.Object[] ItemObject2 = new System.Object[pList.Count];
            //for (int i = 0; i < pList.Count; i++)
            //{
            //    ItemObject2[i] = pList[i];
            //}
            //comboIssue.Items.AddRange(ItemObject2);
            log.Info("Set the dropdown items in the Quick Win form for Problem combo box!");

            // ------------------> comboResolution

            //this.comboResolution.Items.AddRange(new object[] {
            //"Reset Password",
            //"Activate user",
            //"Grant access",
            //"Install Hardware",
            //"Install Software",
            //"Other"});

            var res = spRecordList.Where(p => p.problem == comboIssue.Text).Select(r => r.resolution).Distinct();

            comboResolution.Items.Clear();
            comboResolution.Items.AddRange(res.ToArray());
            comboResolution.Items.Add("Other");

            //System.Object[] ItemObjectR = new System.Object[res.Count() + 1];
            //for (int i = 0; i < res.Count(); i++)
            //{
            //ItemObjectR[i] = res.ElementAt(i);
            //}
            //ItemObjectR[res.Count()] = "Other";
            //comboResolution.Items.Clear();
            //comboResolution.Items.AddRange(ItemObjectR);

            //System.Object[] ItemObject3 = new System.Object[rList.Count];
            //for (int i = 0; i < rList.Count; i++)
            //{
            //    ItemObject3[i] = rList[i];
            //}
            //comboResolution.Items.AddRange(ItemObject3);
            log.Info("Set the dropdown items in the Quick Win form for Resolution combo box!");
        }

        // Submit button in the User form
        // Returns nothing
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            log.Info("Inside simpleButton1_Click to create ticket in SP and then close the Quick Win form!");

            log.Info("Inside QuickWinForm - submit Form to SP button Click!");

            // check if all user fields are filled by the user
            if (String.IsNullOrEmpty(requestedBy) || String.IsNullOrEmpty(source)
                || String.IsNullOrEmpty(system) || String.IsNullOrEmpty(issue)
                || String.IsNullOrEmpty(resolution) )
            {
                string message = "All the fields are mandatory!";
                string message1 = "Kindly fill all fields & try Again!";
                MessageBox.Show(message + '\n' + message1);
            }
            else
            {
                if (String.IsNullOrEmpty(ticketDate))
                {
                    string message = "Kindly select proper date, Thank you!";
                    MessageBox.Show(message);
                    log.Info(message);
                }
                else
                {
                    if (submitFormToSP())
                    {
                        string message = "Ticket created in SharePoint, Thank you!";
                        MessageBox.Show(message);
                        log.Info(message);

                        // making the form instance null before closing it
                        sFormInstance = null;

                        this.Close();
                    }
                    else
                    {
                        string message = "Ticket could not be created in SharePoint, Sorry :(";
                        string message1 = "Kindly try Again!";
                        MessageBox.Show(message + '\n' + message1);
                        log.Info(message + '\n' + message1);
                    }
                }
                
            }
            
        }

        // Function to create an item in SharePoint
        // Returns ticket created status - boolean
        private bool submitFormToSP()
        {
            log.Info("Inside QuickWinForm - submitFormToSP func!");
            bool createStatus = false;

            if (client != null)
            {
                log.Info("Inside QuickWinForm - SP client is not null!");

                spFilledItemObject = setItemFields();
                client.createItem(spFilledItemObject);
                //client.createItem();
                createStatus = true;
            }
            else
            {
                log.Debug("Inside QuickWinForm - SP client is null!");
                createStatus = false;
            }

            return createStatus;
        }

        // Function to set the different fields of the SP item
        // Returns the object of Class SpItemObject containing all item fields
        private SpItemObject setItemFields()
        {
            log.Info("Inside QuickWinForm - setItemFields func!");

            SpItemObject spItemObject = new SpItemObject();

            if (spItemObject != null)
            {
                log.Info("Inside QuickWinForm - SP item object is not null!");
                spItemObject.Date = ticketDate;
                spItemObject.RequestedBy = requestedBy;
                spItemObject.Source = source;
                spItemObject.System = system;
                spItemObject.Problem = issue;
                spItemObject.Other = otherIssue;
                spItemObject.Resolution = resolution;
                spItemObject.OtherResolution = otherResolution;
                spItemObject.Title = system + " - " + issue;
            }
            else
            {
                log.Debug("Inside QuickWinForm - SP item object is null!");
            }

            return spItemObject;
        }

        // Text Field in the User form to fetch the Other Problems/ Issues
        // Returns nothing
        private void memoEdit1_EditValueChanged(object sender, EventArgs e)
        {
            log.Debug("Inside QuickWinForm - memoEdit1_EditValueChanged - text box for other Problem/Issue!");

            if (txtOherIssue.Visible == true)
            {
                otherIssue = txtOherIssue.Text;

                if (string.IsNullOrEmpty(otherIssue))
                {
                    log.Info("otherIssue value is null/blank!");
                    otherIssue = "";
                }
            }
            else
            {
                otherIssue = "";
            }

            log.Info("otherIssue value changed to - " + otherIssue);
        }

        // Combo dropdown field in the User Form to fetch the Problem/Issue input
        // Returns nothing
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setResolDropdownItems();

            issue = comboIssue.Text;

            if (string.IsNullOrEmpty(issue))
            {
                log.Info("issue value is null/blank!");
                issue = "";
            }
            else
            {
                if (issue == "Other")
                {
                    log.Info("Problem/Issue selected is other, so showing text box");
                    txtOherIssue.Visible = true;
                    layoutControlItem11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //label6.Visible = true;
                }
                else
                {
                    txtOherIssue.Visible = false;
                    layoutControlItem11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //label6.Visible = false;
                }
            }

            log.Info("issue value changed to - " + issue);

        }

        // Combo dropdown field in the User Form to set the Resolution options
        // Returns nothing
        private void setResolDropdownItems()
        {
            List<SpRecords> spRecordList = FormDropdownOptions.records;

            var res = spRecordList.Where(p => p.problem == comboIssue.Text).Select(r => r.resolution).Distinct();

            comboResolution.Items.Clear();
            comboResolution.Items.AddRange(res.ToArray());
            comboResolution.Items.Add("Other");

            //System.Object[] ItemObjectR = new System.Object[res.Count() + 1];
            //for (int i = 0; i < res.Count(); i++)
            //{
            //    ItemObjectR[i] = res.ElementAt(i);
            //}
            //ItemObjectR[res.Count()] = "Other";
            //comboResolution.Items.Clear();
            //comboResolution.Items.AddRange(ItemObjectR);

            //System.Object[] ItemObject2 = new System.Object[pList.Count];
            //for (int i = 0; i < pList.Count; i++)
            //{
            //    ItemObject2[i] = pList[i];
            //}
            //comboIssue.Items.AddRange(ItemObject2);
            log.Info("Set the dropdown items in the Quick Win form for Problem combo box!");
        }

        // Combo dropdown field in the User Form to fetch the Source input
        // Returns nothing
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            source = comboSource.Text;

            if (string.IsNullOrEmpty(source))
            {
                log.Info("source value is null/blank!");
                source = "";
            }
            log.Info("source value changed to - " + comboSource.Text);
        }

        // Combo dropdown field in the User Form to fetch the System input
        // Returns nothing
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            system = comboSystem.Text;

            if (string.IsNullOrEmpty(system))
            {
                log.Info("system value is null/blank!");
                system = "";
            }
            log.Info("system value changed to - " + comboSystem.Text);

            setProblemDropdownItems();
        }

        // Combo dropdown field in the User Form to set the Problems/Issues options
        // Returns nothing
        private void setProblemDropdownItems()
        {
            List<SpRecords> spRecordList = FormDropdownOptions.records;

            var prob = spRecordList.Where(s => s.system == comboSystem.Text).Select(p => p.problem).Distinct();

            comboIssue.Items.Clear();
            comboIssue.Items.AddRange(prob.ToArray());
            comboIssue.Items.Add("Other");

            //System.Object[] ItemObjectP = new System.Object[prob.Count() + 1];
            //for (int i = 0; i < prob.Count(); i++)
            //{
            //    ItemObjectP[i] = prob.ElementAt(i);
            //}
            //ItemObjectP[prob.Count()] = "Other";
            //comboIssue.Items.Clear();
            //comboIssue.Items.AddRange(ItemObjectP);

            //System.Object[] ItemObject2 = new System.Object[pList.Count];
            //for (int i = 0; i < pList.Count; i++)
            //{
            //    ItemObject2[i] = pList[i];
            //}
            //comboIssue.Items.AddRange(ItemObject2);
            log.Info("Set the dropdown items in the Quick Win form for Problem combo box!");
        }

        // Combo dropdown field in the User Form to fetch the Resolution input
        // Returns nothing
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            resolution = comboResolution.Text;

            if (string.IsNullOrEmpty(resolution))
            {
                log.Info("resolution value is null/blank!");
                resolution = "";
            }
            else
            {
                if (resolution == "Other")
                {
                    log.Info("resolution to Problem/Issue selected is other, so showing text box");
                    txtOtherResolution.Visible = true;
                    layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //label6.Visible = true;
                }
                else
                {
                    txtOtherResolution.Visible = false;
                    layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //label6.Visible = false;
                }
            }

            log.Info("resolution value changed to - " + resolution);
        }

        // Text Field in the User form to fetch the Other Resolution
        // Returns nothing
        private void memoEdit2_EditValueChanged(object sender, EventArgs e)
        {
            log.Debug("Inside QuickWinForm - memoEdit2_EditValueChanged - text box for other Resolution!");

            if (txtOtherResolution.Visible == true)
            {
                otherResolution = txtOtherResolution.Text;

                if (string.IsNullOrEmpty(otherResolution))
                {
                    log.Info("otherResolution value is null/blank!");
                    otherResolution = "";
                }
            }
            else
            {
                otherResolution = "";
            }

            log.Info("otherResolution value changed to - " + otherResolution);
        }

        // Function to initialize all the variables in the class
        // Returns nothing
        private void InitializeVariables()
        {
            log.Info("Inside QuickWinForm - InitializeVariables func!");
            requestedBy = "";
            source = "";
            system = "";
            issue = "";
            otherIssue = "";
            resolution = "";
            otherResolution = "";
            ticketDate = DateTime.Now.ToShortDateString();
        }

        // Cancel button in the User form
        // Returns nothing
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            log.Info("Inside QuickWinForm - cancel button is clicked!");
            
            // making the instance as null before closing
            sFormInstance = null;

            this.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Inside QuickWinForm - dateTimePicker1_ValueChanged!");
            if (dateTimePicker1.Value > DateTime.Now)
            {
                string message = "Kindly select past or today's date, Thank you!";
                MessageBox.Show(message);
                log.Info(message);
                ticketDate = "";
            }
            else
            {
                ticketDate = dateTimePicker1.Value.ToShortDateString();
                log.Info("Inside QuickWinForm - dateTimePicker1_ValueChanged! - date set to - " + ticketDate);
            }
            
        }

        // Ellipsis button click in the User form to open the Dialog box to fetch the requested by name from AD
        // Returns nothing
        private void txtRequestedBy_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            log.Debug("Inside QuickWinForm - txtRequestedBy_ButtonClick - Ellipsis text box for RequestedBy!");

            var edit = (ButtonEdit)sender;
            switch (e.Button.Kind)
            {
                case ButtonPredefines.Ellipsis:
                    using (var dlg = new ActiveDirectoryHelper.ActiveDirectory.DirectoryObjectPickerDialog())
                    {
                        dlg.AllowedLocations = ActiveDirectoryHelper.ActiveDirectory.Locations.EnterpriseDomain;
                        dlg.AllowedObjectTypes = ActiveDirectoryHelper.ActiveDirectory.ObjectTypes.Users;
                        dlg.DefaultLocations = dlg.AllowedLocations;
                        dlg.DefaultObjectTypes = dlg.AllowedObjectTypes;
                        dlg.MultiSelect = false;
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            edit.EditValue = dlg.SelectedObject;
                        }
                    }
                    txtRequestedBy.Text = edit.EditValue.ToString();
                    requestedBy = edit.EditValue.ToString();
                    //textEdit1.Text = txtRequestedBy.Text;
                    break;
            }
            //    Case ButtonPredefines.Ellipsis
            //        'Browse
            //        Using dlg As New ActiveDirectory.DirectoryObjectPickerDialog
            //            dlg.AllowedLocations = ActiveDirectory.Locations.EnterpriseDomain
            //            dlg.AllowedObjectTypes = ActiveDirectory.ObjectTypes.Users
            //            dlg.DefaultLocations = dlg.AllowedLocations
            //            dlg.DefaultObjectTypes = dlg.AllowedObjectTypes
            //            dlg.MultiSelect = False
            //            If dlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            //                edit.EditValue = dlg.SelectedObject
            //            End If
            //        End Using
            //    Case Else
            //        'do nothing
            //End Select
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            log.Debug("Inside QuickWinForm - lookUpEdit1_EditValueChanged - for RequestedBy!");

            // To get the requester's name from Active Directory
            var edit = (ButtonEdit)sender;
            using (var dlg = new ActiveDirectoryHelper.ActiveDirectory.DirectoryObjectPickerDialog())
            {
                dlg.AllowedLocations = ActiveDirectoryHelper.ActiveDirectory.Locations.EnterpriseDomain;
                dlg.AllowedObjectTypes = ActiveDirectoryHelper.ActiveDirectory.ObjectTypes.Users;
                dlg.DefaultLocations = dlg.AllowedLocations;
                dlg.DefaultObjectTypes = dlg.AllowedObjectTypes;
                dlg.MultiSelect = false;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    edit.EditValue = dlg.SelectedObject;
                }
            }

        }

        // Requested by text field in the User form to fetch the user name from AD
        // Returns nothing
        private void txtRequestedBy_EditValueChanged(object sender, EventArgs e)
        {
            log.Debug("Inside QuickWinForm - txtRequestedBy_EditValueChanged - setting value for RequestedBy!");
            requestedBy = txtRequestedBy.Text;
        }

        // Requested by text field in the User form to monitor the space key press to fetch the user name from AD
        // Returns nothing
        private void txtRequestedBy_KeyUp(object sender, KeyEventArgs e)
        {
            log.Debug("Inside QuickWinForm - txtRequestedBy_KeyUp - space key pressed for RequestedBy!");
            if (e.KeyCode == Keys.Space)
            {
                //txtRequestedBy_ButtonClick(sender, e);

                var edit = (ButtonEdit)sender;
                using (var dlg = new ActiveDirectoryHelper.ActiveDirectory.DirectoryObjectPickerDialog())
                {
                    dlg.AllowedLocations = ActiveDirectoryHelper.ActiveDirectory.Locations.EnterpriseDomain;
                    dlg.AllowedObjectTypes = ActiveDirectoryHelper.ActiveDirectory.ObjectTypes.Users;
                    dlg.DefaultLocations = dlg.AllowedLocations;
                    dlg.DefaultObjectTypes = dlg.AllowedObjectTypes;
                    dlg.MultiSelect = false;
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        edit.EditValue = dlg.SelectedObject;
                    }
                }
                txtRequestedBy.Text = edit.EditValue.ToString();
                requestedBy = edit.EditValue.ToString();
                log.Debug("Inside QuickWinForm - txtRequestedBy_KeyUp - RequestedBy!" + requestedBy);
            }
        }
    }
}
