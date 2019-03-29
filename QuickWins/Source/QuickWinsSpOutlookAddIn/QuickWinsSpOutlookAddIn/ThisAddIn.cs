using log4net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace QuickWinsSpOutlookAddIn
{
    public partial class ThisAddIn
    {

        private static ILog log = LogManager.GetLogger(typeof(ThisAddIn));
        CreateSpListItem client;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            log.Info("Inside ThisAddIn_Startup!");
            log4net.Config.XmlConfigurator.Configure();

            // checking if the currently signed in User belongs to specific user groups
            //var status = IsUserInGroup("IS_HelpDesk");
            //var status = IsUserInGroup("LOCAL");

            //var status = IsUserInGroup("een\\IT Support - Apps");
            var status = checkUserGroup("IT Support - Apps");
            //var status = IsUserInGroup(Properties.Settings.Default.QwAllowedUserGroup);
            //var status = checkUserGroup(Properties.Settings.Default.QwAllowedUserGroup);
            if (!status)
            {
                // if the User is not in the mentioned group, then hide the button
                //disableRibbonButton();
                removeRibbonButton();
            }

            client = new CreateSpListItem();

            // retrieve the items from QW Option list here and save it in object
            getSpListData();
        }


        // Function to disable the Ribbon Button if the User does not belong to the provided group name
        // Returns nothing
        private void disableRibbonButton()
        {
            Type type = typeof(Ribbon1);
            Ribbon1 ribbon = Globals.Ribbons.GetRibbon(type) as Ribbon1;
            ribbon.btnForm.Enabled = false;
        }

        // Function to remove the Ribbon Button if the User does not belong to the provided group name
        // Returns nothing
        private void removeRibbonButton()
        {
            Type type = typeof(Ribbon1);
            Ribbon1 ribbon = Globals.Ribbons.GetRibbon(type) as Ribbon1;
            ribbon.btnForm.Visible = false;
        }


        // Function to check if the User belongs to the provided group name
        // Returns true if the User belongs to groupName, or false
        private bool checkUserGroup(string groupName)
        {
            var groups = UserPrincipal.Current.GetGroups();
            //var group = WindowsIdentity.GetCurrent().Groups;
            //
            //var result = group.Any(x => x.SamAccountName(""));

            var result = groups.Any(x => x.SamAccountName.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));

            return result;
            //return false;
        }


        // Function to check if the User belongs to the provided group name
        // Returns true if the User belongs to groupName
        private bool IsUserInGroup(string groupName)
        {
            System.Diagnostics.Debug.WriteLine(groupName);

            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();

            IdentityReferenceCollection userGroups = currentUser.Groups;

            foreach (IdentityReference group in userGroups)
            {
                IdentityReference translated = group.Translate(typeof(NTAccount));

                System.Diagnostics.Debug.WriteLine(translated.Value);
                if (translated.Value.Contains(groupName))
                {
                    return true;
                }

                if (groupName.Equals(translated.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        // Function to Retrieve QWOption List Items from SharePoint
        // Returns nothing
        private void getSpListData()
        {
            //CreateSpListItem client = new CreateSpListItem();
            if (client != null)
            {
                IRestResponse<RootObject> qwRespObj = client.RetrieveQWOptionItems();
                createNewLists(qwRespObj);
            }
        }


        // Function to add the QWOption List Items retrieved from SharePoint
        // This function adds to each SP item as an object to the Class FormDropdownOptions.records 
        // Returns nothing
        private void createNewLists(IRestResponse<RootObject> respObj)
        {
            if (respObj != null)
            {
                log.Info("Response after SP list items retrieval is not Null!");

                //FormDropdownOptions dropdownOptions = new FormDropdownOptions();

                //List<string> sList = new List<string>();
                //List<string> pList = new List<string>();
                //List<string> rList = new List<string>();

                //sList = dropdownOptions.systemList;
                //pList = dropdownOptions.problemList;
                //rList = dropdownOptions.resolutionList;

                //dropdownOptions.systemList = new List<string>();

                FormDropdownOptions.systemList = new List<string>();
                FormDropdownOptions.problemList = new List<string>();
                FormDropdownOptions.resolutionList = new List<string>();
                FormDropdownOptions.records = new List<SpRecords>();

                int count = respObj.Data.d.results.Count;
                log.Info("Count of Response after SP list items retrieval is - " + count);

                for (int j = 0; j < count; j++)
                {
                    log.Info("Index of Response after SP list items retrieval - " + j);

                    var obj = new SpRecords();

                    bool status = respObj.Data.d.results[j].Active;
                    log.Info("Status of Response after SP list items retrieval at index - " + j + " , is - " + status);

                    obj.status = status;

                    if (status)
                    {
                        string title = respObj.Data.d.results[j].Title;
                        log.Info("System of Response after SP list items retrieval at index - " + j + " , is - " + title);
                        //sList.Add(title);
                        //if (dropdownOptions.systemList.Contains(title))
                        //{
                        //    log.Info("dropdownOptions.systemList Contains title - " + title + " ,at index - " + j);
                        //}
                        //else
                        //{
                        //    dropdownOptions.systemList.Add(title);
                        //    log.Info("Add title - " + title + " ,at index - " + j + " to dropdownOptions.systemList.");
                        //}

                        if (FormDropdownOptions.systemList.Contains(title))
                        {
                            log.Info("dropdownOptions.systemList Contains title - " + title + " ,at index - " + j);
                        }
                        else
                        {
                            FormDropdownOptions.systemList.Add(title);

                            log.Info("Add title - " + title + " ,at index - " + j + " to dropdownOptions.systemList.");
                        }
                        obj.system = title;

                        string problem = respObj.Data.d.results[j].Problem;
                        log.Info("problem of Response after SP list items retrieval at index - " + j + " , is - " + problem);
                        //pList.Add(problem);
                        if (FormDropdownOptions.problemList.Contains(problem))
                        {
                            log.Info("dropdownOptions.problemList Contains problem - " + problem + " ,at index - " + j);
                        }
                        else
                        {
                            FormDropdownOptions.problemList.Add(problem);

                            log.Info("Add problem - " + problem + " ,at index - " + j + " to dropdownOptions.problemList.");
                        }
                        obj.problem = problem;

                        string resolution = respObj.Data.d.results[j].Resolution;
                        log.Info("resolution of Response after SP list items retrieval at index - " + j + " , is - " + resolution);
                        //rList.Add(resolution);
                        if (FormDropdownOptions.resolutionList.Contains(resolution))
                        {
                            log.Info("dropdownOptions.resolutionList Contains resolution - " + resolution + " ,at index - " + j);
                        }
                        else
                        {
                            FormDropdownOptions.resolutionList.Add(resolution);

                            log.Info("Add resolution - " + resolution + " ,at index - " + j + " to dropdownOptions.resolutionList.");
                        }
                        obj.resolution = resolution;

                        FormDropdownOptions.records.Add(obj);
                    }
                }

                // Adding "Other" option 
                FormDropdownOptions.problemList.Add("Other");
                FormDropdownOptions.resolutionList.Add("Other");
            }
        }

        // Dummy test function
        private void createNewLists1(IRestResponse<RootObject> respObj)
        {
            if (respObj != null)
            {
                log.Info("Response after SP list items retrieval is not Null!");

                //FormDropdownOptions dropdownOptions = new FormDropdownOptions();

                //List<string> sList = new List<string>();
                //List<string> pList = new List<string>();
                //List<string> rList = new List<string>();

                //sList = dropdownOptions.systemList;
                //pList = dropdownOptions.problemList;
                //rList = dropdownOptions.resolutionList;

                //dropdownOptions.systemList = new List<string>();

                FormDropdownOptions.systemList = new List<string>();
                FormDropdownOptions.problemList = new List<string>();
                FormDropdownOptions.resolutionList = new List<string>();

                int count = respObj.Data.d.results.Count;
                log.Info("Count of Response after SP list items retrieval is - " + count);

                for (int j = 0; j < count; j++)
                {
                    log.Info("Index of Response after SP list items retrieval - " + j);

                    bool status = respObj.Data.d.results[j].Active;
                    log.Info("Status of Response after SP list items retrieval at index - " + j + " , is - " + status);

                    if (status)
                    {
                        string title = respObj.Data.d.results[j].Title;
                        log.Info("System of Response after SP list items retrieval at index - " + j + " , is - " + title);
                        //sList.Add(title);
                        //if (dropdownOptions.systemList.Contains(title))
                        //{
                        //    log.Info("dropdownOptions.systemList Contains title - " + title + " ,at index - " + j);
                        //}
                        //else
                        //{
                        //    dropdownOptions.systemList.Add(title);
                        //    log.Info("Add title - " + title + " ,at index - " + j + " to dropdownOptions.systemList.");
                        //}

                        if (FormDropdownOptions.systemList.Contains(title))
                        {
                            log.Info("dropdownOptions.systemList Contains title - " + title + " ,at index - " + j);
                        }
                        else
                        {
                            FormDropdownOptions.systemList.Add(title);
                            log.Info("Add title - " + title + " ,at index - " + j + " to dropdownOptions.systemList.");
                        }

                        string problem = respObj.Data.d.results[j].Problem;
                        log.Info("problem of Response after SP list items retrieval at index - " + j + " , is - " + problem);
                        //pList.Add(problem);
                        if (FormDropdownOptions.problemList.Contains(problem))
                        {
                            log.Info("dropdownOptions.problemList Contains problem - " + problem + " ,at index - " + j);
                        }
                        else
                        {
                            FormDropdownOptions.problemList.Add(problem);
                            log.Info("Add problem - " + problem + " ,at index - " + j + " to dropdownOptions.problemList.");
                        }

                        string resolution = respObj.Data.d.results[j].Resolution;
                        log.Info("resolution of Response after SP list items retrieval at index - " + j + " , is - " + resolution);
                        //rList.Add(resolution);
                        if (FormDropdownOptions.resolutionList.Contains(resolution))
                        {
                            log.Info("dropdownOptions.resolutionList Contains resolution - " + resolution + " ,at index - " + j);
                        }
                        else
                        {
                            FormDropdownOptions.resolutionList.Add(resolution);
                            log.Info("Add resolution - " + resolution + " ,at index - " + j + " to dropdownOptions.resolutionList.");
                        }
                    }
                }

                // Adding "Other" option 
                FormDropdownOptions.problemList.Add("Other");
                FormDropdownOptions.resolutionList.Add("Other");
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            log.Info("Inside ThisAddIn_Shutdown!");
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            log.Info("Inside InternalStartup!");
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
