using System;
using System.Collections.Generic;
using log4net;
using RestSharp;

namespace QuickWinsSpOutlookAddIn
{
    public class CreateSpListItem
    {

        private static ILog log = LogManager.GetLogger(typeof(CreateSpListItem));

        private RestClient client;
        private string listName1;
        private string listName2;
        //private string folder1;
        //private string folder2;

        // Constructor to create the client object and
        // Set the variables' values.
        public CreateSpListItem()
        {
            log.Info("Inside CreateSpListItem constructor to create Client!");

            //SimpleJson.CurrentJsonSerializerStrategy = new CustomJsonSerializerStrategy();

            listName1 = "QuickWinsList";
            listName2 = "QWOptionsList";
            //string folder1 = "QuickWins";
            //string folder2 = "QWOptions";
            string ServerUrl = "http://pacenet";
            string SiteUrl = "home/it";

            // client = new RestClient($@"{ServerUrl}/{SiteUrl}/_api/web/lists/{listName1}");
            client = new RestClient($@"{ServerUrl}/{SiteUrl}/_api/");
        }

        // Function to connect to SharePoint via REST API, 
        // and create an item in SP with the field items entered by User via Quick Wins Form
        // Returns nothing
        public void createItem(SpItemObject spItemObject)
        {
            log.Info("Inside createItem to create List Items!");

            //string SiteUrl = "http://pacenet/home/it/_api/web/lists/QuickWinsList";

            var digestValue = GetDigestValue();
            if (digestValue == null || digestValue == "") {
                //digestValue = "0x9BD9391FD3C91832C4157FB5E64355309536E0341BB7B76A329639CCD72AD772C11D0D8FE157860A12BA88DE225DFAAD24C653DFF209D7C3C2EEA1AFFDE02F51,13 Mar 2019 12:47:52 -0000";
            }

            //var req = new RestRequest(SiteUrl, Method.POST)
            //var req = new RestRequest($"{ SiteUrl}/Items", Method.POST)
            var req = new RestRequest($"web/lists/{listName1}/Items", Method.POST)
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials,
                RequestFormat = DataFormat.Json
            };
            req.AddHeader("Accept", "application/json;odata=verbose");
            //req.AddHeader("Content-Type", "application/json;odata=verbose");
            //req.AddHeader("ContentLength", "400");
            req.AddHeader("X-RequestDigest", digestValue);
            req.JsonSerializer.ContentType = "application/json;odata=verbose";

            var item = new Model.ListItems.BaseResult();
            item.Title = spItemObject.Title;
            item.Date = spItemObject.Date;
            item.RequestedBy = spItemObject.RequestedBy;
            item.Source = spItemObject.Source;
            item.System = spItemObject.System;
            item.Problem = spItemObject.Problem;
            item.Other = spItemObject.Other;
            item.Resolution = spItemObject.Resolution;
            item.OtherResolution = spItemObject.OtherResolution;

            item.__metadata = new Model.ListItems.Metadata() { type = "SP.Data.QuickWinsListItem" };

            req.AddJsonBody(item);

            var response = client.Execute(req);

            log.Debug("Creating SP list item!");
        }

        // Dummy function for Testing
        public void createItem1(SpItemObject spItemObject)
        {
            log.Info("Inside createItem to create List Items!");

            //string SiteUrl = "http://pacenet/home/it/_api/web/lists/QuickWinsList";

            var digestValue = GetDigestValue();

            //var req = new RestRequest(SiteUrl, Method.POST)
            //var req = new RestRequest($"{ SiteUrl}/Items", Method.POST)
            var req = new RestRequest($"web/lists/{listName1}/Items", Method.POST)
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials,
                RequestFormat = DataFormat.Json
            };
            req.AddHeader("Accept", "application/json;odata=verbose");
            //req.AddHeader("Content-Type", "application/json;odata=verbose");
            //req.AddHeader("ContentLength", "400");
            req.AddHeader("X-RequestDigest", digestValue);
            req.JsonSerializer.ContentType = "application/json;odata=verbose";

            var item = new Model.ListItems.BaseResult();
            item.Title = "TEST 1";
            item.Date = "";
            item.RequestedBy = "";
            item.Source = "";
            item.System = "";
            item.Problem = "";
            item.Other = "";
            item.Resolution = "";
            item.OtherResolution = "";

            item.__metadata = new Model.ListItems.Metadata() { type = "SP.Data.QuickWinsListItem" };

            req.AddJsonBody(item);

            var response = client.Execute(req);

            log.Debug("creating SP list item!");
        }

        // Function to connect to SharePoint via REST API, 
        // and fetch all items in SP to provide dropdown options to User in the Quick Wins Form
        // Returns RootObject of the REST Response - object contains all the fetched SP Items
        public IRestResponse<RootObject> RetrieveQWOptionItems()
        {
            log.Info("Inside RetrieveQWOptionItems to retrieve QW Option List Items!");

            var digestValue = GetDigestValue();

            var req = new RestRequest($"web/lists/{listName2}/Items", Method.GET)
            //var req = new RestRequest($"lists/getbytitle('{folder}')/GetItems", Method.POST)
            //var req = new RestRequest($"{ServerUrl}/{SiteUrl}/_api/web/lists/{listName}/Items", Method.POST)
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials,
                RequestFormat = DataFormat.Xml
            };
            req.AddHeader("Accept", "application/json;odata=verbose");
            req.AddHeader("X-RequestDigest", digestValue);
            req.JsonSerializer.ContentType = "application/json;odata=verbose";

            //var resp = client.Execute(req);
            IRestResponse<RootObject> respObj = client.Execute<RootObject>(req);

            if (respObj !=null)
            {
                log.Info("Response after SP list items retrieval is not Null!");

                int count = respObj.Data.d.results.Count;
                log.Info("Count of Response after SP list items retrieval is - " + count);

                // List of String to hold the values of System name, Problems and Resolution
                List<string> systemList = new List<string>();
                List<string> problemList = new List<string>();
                List<string> resolutionList = new List<string>();

                for (int j =0; j < count; j++)
                {
                    log.Info("Index of Response after SP list items retrieval - " + j);

                    bool status = respObj.Data.d.results[j].Active;
                    log.Info("Status of Response after SP list items retrieval at index - " + j + " , is - " + status);

                    if (status)
                    {
                        string title = respObj.Data.d.results[j].Title;
                        log.Info("System of Response after SP list items retrieval at index - " + j + " , is - " + title);
                        systemList.Add(title);

                        string problem = respObj.Data.d.results[j].Problem;
                        log.Info("problem of Response after SP list items retrieval at index - " + j + " , is - " + problem);
                        problemList.Add(problem);

                        string resolution = respObj.Data.d.results[j].Resolution;
                        log.Info("resolution of Response after SP list items retrieval at index - " + j + " , is - " + resolution);
                        resolutionList.Add(resolution);

                    }
                }
            }
            //string title = respObj.Data.d.results[0].Title;
            //string problem = respObj.Data.d.results[0].Problem;
            //string resolution = respObj.Data.d.results[0].Resolution;
            //bool status = respObj.Data.d.results[0].Active;

            //string problem2 = respObj.Data.d.results[1].Problem;
            //string resolution2 = respObj.Data.d.results[1].Resolution;
            //string resolution3 = respObj.Data.d.results[2].Resolution;

            // Loop through the object to get all the values in the list

            return respObj;
        }

        // Function to connect to SharePoint via REST API, 
        // and fetch Digest Value required to connect to SP
        // Returns Digest Value - String
        private string GetDigestValue()
        {
            log.Info("Inside GetDigestValue to Get the DigestValue!");

            var req = new RestRequest("contextinfo?$select=FormDigestValue", Method.POST)
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials,
                UseDefaultCredentials = true
            };
            req.AddHeader("Accept", "application/json;odata=verbose");
            var val = client.Execute<Model.ContextInformation.RootObject>(req);
            if (HasError(val, System.Net.HttpStatusCode.OK)) return "";
            return val?.Data?.d?.GetContextWebInformation?.FormDigestValue ?? "";
        }

        // Function to check if error has occurred
        // Returns error status - boolean
        private bool HasError(IRestResponse response, System.Net.HttpStatusCode expectedResponse)
        {
            log.Info("Inside HasError func!");

            if (response.ErrorException != null)
            {
                OnErrorOccurred(response.ErrorException);
                return true;
            }
            else if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                OnErrorOccurred(response.ErrorMessage);
                return true;
            }
            else if (response.StatusCode != expectedResponse)
            {
                OnErrorOccurred(response.StatusDescription);
                return true;
            }

            return false;
        }

        // Function to get the error message if error has occurred
        // Returns error message - string
        private void OnErrorOccurred(string errorMessage)
        {
            log.Info("Inside OnErrorOccurred for error message func!");

            ErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs(errorMessage));
        }

        // Function to get the error exception if error has occurred
        // Returns nothing
        private void OnErrorOccurred(Exception errorException)
        {
            log.Info("Inside OnErrorOccurred for exception func!");

            ErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs(errorException));
        }

        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;


    }
}
