using System;
using log4net;
using RestSharp;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace GetCallRailData
{
    class Program
    {
        private const string apiKey = "620ddf5b5f5860ccd0ce7976054b86bd";
        private const string baanTableName = "ttccom906260";
        private const string accountID = "638986454";
        private static ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            try
            {
                log.Debug("Inside Main function - Start of GetCallRailData Application");
                //for testing function
                //int locRegion = getlocRegFromName("Digital Paid Media Pool_266260");
                // Get the latest CallRail data using their API
                List<IRestResponse<RootObject>> response = getDataFromCallrail();
                if (response == null || response.Count == 0)
                {
                    log.Debug("Data not fetched from Call Rail");
                    //Console.WriteLine("Data cannot be fetched from CallRail");
                }
                else
                {
                    log.Info("Data fetched successfully from Call Rail");
                    Console.WriteLine("Data fetched successfully from CallRail");

                    // Update the Baan table with the fetched CallRail data
                    int baanUpdateStatus = updateBaanTable(response);
                    if (baanUpdateStatus <= 0)
                    {
                        log.Debug("Baan table - " + baanTableName  + " , update failed!");
                        //Console.WriteLine("Baan table update failed!");
                    }
                    else
                    {
                        log.Info("Baan table updated successfully for  table - " + baanTableName + " , for # of records - " + baanUpdateStatus);
                        Console.WriteLine("Baan table updated successfully for  table - " + baanTableName + " , for # of records - " + baanUpdateStatus);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("Exception in Main program occurred! " + e);
                Console.WriteLine("Exception in Main program occurred! " + e);
            }
            finally
            {
#if DEBUG
                Console.WriteLine("In DEBUG - Press any key...");
                Console.ReadKey();
#endif
            }
        }

        static List<IRestResponse<RootObject>> getDataFromCallrail()
        {
            log.Debug("Inside getDataFromCallrail function.");
            List<IRestResponse<RootObject>> objectList = new List<IRestResponse<RootObject>>();
            IRestResponse<RootObject> response = null;
            IRestResponse<RootObject> responseFromList = null;

            // API to fetch CallRail data
            // The client url is for individual page, so initializing the page to 1 
            // and then looping through other pages, based on page count
            string pageCount = "1";
            //var client = new RestSharp.RestClient(Properties.Settings.Default.listAllCalls);
            var client = new RestSharp.RestClient("https://api.callrail.com/v2/a/{account_id}/calls.json?fields=source_name,keywords&page=" + pageCount);
            var request = new RestSharp.RestRequest(RestSharp.Method.GET);

            request.AddHeader("Authorization", "Token token=" + apiKey);
            request.AddParameter("account_id", accountID, ParameterType.UrlSegment);
            response = client.Execute<RootObject>(request);


            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Accepted ||
                 response.StatusCode == HttpStatusCode.Created) & response != null)
            {
                log.Debug("Inside response okay for page 1.");
                objectList.Add(response);
                log.Info("Adding response to the response list for page number -" + pageCount);
                for (int pageNumber = 2; pageNumber <= response.Data.total_pages; pageNumber++)
                {
                    log.Debug("Inside for loop for all pages.");
                    client = new RestSharp.RestClient("https://api.callrail.com/v2/a/{account_id}/calls.json?fields=source_name,keywords&page=" + pageNumber);
                    response = client.Execute<RootObject>(request);
                    if ((response.StatusCode == HttpStatusCode.OK || 
                        response.StatusCode == HttpStatusCode.Accepted ||
                        response.StatusCode == HttpStatusCode.Created) & response != null)
                    {
                        log.Debug("Inside response okay for page - " + pageNumber);
                        objectList.Add(response);
                        log.Info("Adding response to the response list for page number -" + pageNumber);
                    }
                    else
                    {
                        log.Debug("Inside response NOT okay for page - " + pageNumber);
                        log.Info("Could not Add response to the response list for page number -" + pageNumber);
                        log.Error("Response status code is - " + response.StatusCode + " - at page number - " + pageNumber);
                    }
                        
                }
                if (objectList != null && objectList.Count > 0)
                {
                    log.Debug("Count of objectList is - " + objectList.Count);
                    for (int index = 0; index < objectList.Count; index++)
                    {
                        responseFromList = objectList[index];
                        log.Info("Referring object number - " + index + " - from the list of " + objectList.Count);

                        // Just testing out the customer names, print them out through loops
                        switch (responseFromList.StatusCode)
                        {
                            case System.Net.HttpStatusCode.Unauthorized:
                            case HttpStatusCode.NotFound:
                                log.Info("Inside case NotFound!");
                                log.Error("Response status code is - " + responseFromList.StatusCode);
                                break;
                            case HttpStatusCode.OK:
                            case HttpStatusCode.Created:
                            case HttpStatusCode.Accepted:
                                log.Info("Inside case OK!");

                                log.Info("Response here status code is - " + responseFromList.StatusCode);
                                /*
#if DEBUG
                                // Download the data in a file to be used to update Baan table later
                                byte[] responseFile = client.DownloadData(request);
                                File.WriteAllBytes("C:/Users/13519/Documents/callRail/Output/file.xml", responseFile);
#endif
                                 */
                                    // Get each call list of response from the list of response
                                    var callCount = responseFromList.Data.calls.Count;
                                    log.Info("Count of call data list is - " + callCount);
                                    log.Info("Getting in the loop to get the call data at each indexes.");
                                    for (int callIndex = 0; callIndex < callCount; callIndex++)
                                    {
                                        string cuName = responseFromList.Data.calls[callIndex].customer_name;
                                        log.Info("cus name for index -" + callIndex + " is - " + cuName);
                                    }

                                break;
                            case HttpStatusCode.InternalServerError:
                                log.Info("Inside case InternalServerError!");
                                log.Info("Response status code is - " + responseFromList.StatusCode);
                                break;
                            default:
                                //Nothing since something went wrong, just display error message to user
                                log.Info("Inside case Default!");
                                log.Error("Response status code is - " + responseFromList.StatusCode);
                                break;
                        }
                    }
                   
                }
                else
                {
                    Console.WriteLine("Response object list is Null !");
                    log.Info("Response object list is Null!");
                    
                }

           }
            else if (response.StatusCode.Equals("429 Too Many Requests"))
            {
                log.Error("Rate Limit Exceeded: You have used 100 of your 100 Hourly requests at page number 1");
                
            }
            else 
            {
                Console.WriteLine("Response is Null for page 1!");
                log.Info("Response is Null for page 1!");
                log.Error("Response status code is - " + response.StatusCode + " - at page number 1");
            }

            return objectList;
        }


        static int getCompanyNumberFromName(string companyName)
        {
            log.Debug("Inside getCompanyNumberFromName function.");
            int compNum = -1;
            if (string.IsNullOrEmpty(companyName))
            {
                log.Info("companyName is null.");
            }
            else
            {
                log.Info("Company Name for extraction is - " + companyName);
                log.Debug("I am here 17.");

                string company = "";
                log.Info("Length of the company name is -" + companyName.Length);
                if (companyName.Contains("_"))
                {
                    log.Info("company name is -" + companyName + " - contains underscore");
                    int underscoreIndex = companyName.IndexOf("_");

                    log.Info("Position of underscore is -" + underscoreIndex);
                    if (underscoreIndex > 0)
                    {

                        //company = companyName.Substring(underscoreIndex + 1, companyName.Length - underscoreIndex - 1);
                        company = companyName.Substring(underscoreIndex + 1, 3);
                        if (company == "271898")
                        {
                            // Testing comp number extraction
                            log.Info("Company Number extracted is - " + company);
                        }
                        log.Info("Company Number extracted is - " + company);
                    }
                    log.Debug("I am here 18.");
                    try
                    {
                        compNum = System.Convert.ToInt32(company);
                    }
                    catch (FormatException)
                    {
                        // the FormatException is thrown when the string text does 
                        // not represent a valid integer.
                        log.Error("Format Exception to convert string to int for company!");
                    }
                    catch (OverflowException)
                    {
                        // the OverflowException is thrown when the string is a valid integer, 
                        // but is too large for a 32 bit integer.  Use Convert.ToInt64 in
                        // this case.
                        log.Error("Overflow Exception to convert string to int for company!");
                    }
                }
                else
                {
                    log.Info("company name is -" + companyName + " - does not contain underscore");
                }
            }

            return compNum;
        }


        static string getCallDate(string myDate)
        {
            log.Debug("Inside getCallDate function.");
            log.Debug("I am here 17.2.");
            string extractedCallDate = null;
            if (string.IsNullOrEmpty(myDate))
            {
                log.Info("my Date is null.");
            }
            else
            {
                log.Info("my date is -" + myDate);
                log.Info("Length of the company name is -" + myDate.Length);
                if (myDate.Contains("T"))
                {
                    log.Info("date is -" + myDate + " - contains T.");
                    int underscoreIndex = myDate.IndexOf("T");

                    log.Info("Position of underscore is -" + underscoreIndex);
                    if (underscoreIndex > 0)
                    {
                        extractedCallDate = myDate.Substring(0, underscoreIndex);
                        log.Info("Date extracted is - " + extractedCallDate);
                    }
                }
                else
                {
                    log.Info("date is -" + myDate + " - does not contain T.");
                }
            }

            log.Debug("I am here 18.2.");
            return extractedCallDate;
        }

        static string formatKeyword(string unformattedkeyword)
        {
            log.Debug("Inside formatKeyword function.");
            string keyword = null;
            if (string.IsNullOrEmpty(unformattedkeyword))
            {
                log.Info("unformatted keyword is null.");
            }
            else
            {
                log.Info("unformatted keyword is -" + unformattedkeyword);
                log.Info("Length of the unformattedkeyword is -" + unformattedkeyword.Length);
                if (unformattedkeyword.Contains("+") && unformattedkeyword != null && unformattedkeyword != "")
                {
                    log.Info("unformatted keyword is -" + unformattedkeyword + " - contains plus sign.");
                    int underscoreIndex = unformattedkeyword.IndexOf("+");

                    log.Info("Position of plus sign is -" + underscoreIndex);
                    if (underscoreIndex == 0)
                    {
                        keyword = unformattedkeyword.Substring(1);
                        log.Info("keyword extracted from unformattedkeyword is - " + keyword);
                    }
                }
                else
                {
                    log.Info("unformatted keyword is -" + unformattedkeyword + " - does not contain plus sign.");
                }
            }
            return keyword;
        }

        static string getmediaSourceFromName(string companyName)
        {
            log.Debug("Inside getmediaSourceFromName function.");
            log.Debug("I am here 17.3.");
            string mediaSource = null;
            if (string.IsNullOrEmpty(companyName))
            {
                log.Info("companyName is null.");
            }
            else
            {
                log.Info("company name is -" + companyName);
                log.Info("Length of the company name is -" + companyName.Length);
                if (companyName.Contains("_"))
                {
                    log.Info("company name is -" + companyName + " - contains underscore");
                    int underscoreIndex = companyName.IndexOf("_");

                    log.Info("Position of underscore is -" + underscoreIndex);
                    if (underscoreIndex > 0)
                    {
                        mediaSource = companyName.Substring(0, underscoreIndex);
                        log.Info("media Source name extracted is - " + mediaSource);
                    }
                }
                else
                {
                    log.Info("company name is -" + companyName + " - does not contain underscore");
                    mediaSource = companyName;
                    log.Info("mediaSource is company name -" + mediaSource);
                }
            }

            log.Debug("I am here 18.3.");
            return mediaSource;
        }

        
        static string getlocRegFromName(string companyName)
        {
            log.Debug("Inside getlocRegFromName function.");
            log.Debug("I am here 17.1.");
            string locReg = "";
            if (string.IsNullOrEmpty(companyName))
            {
                log.Info("companyName is null.");
            }
            else
            {
                int underscoreIndex = companyName.IndexOf("_");
                log.Info("company name is -" + companyName);
                log.Info("Length of the company name is -" + companyName.Length);
                log.Info("Position of underscore is -" + underscoreIndex);
                if (underscoreIndex > 0)
                {
                    //locReg = companyName.Substring(underscoreIndex + 4, companyName.Length - 1);
                    locReg = companyName.Substring(underscoreIndex + 4);
                    log.Info("location region extracted is - " + locReg);
                }
            }
            log.Debug("I am here 18.1.");
            return locReg;
        }

        static string formatContact(string custContact)
        {
            log.Debug("Inside formatContact function.");
            string formattedContact = "";
            if (string.IsNullOrEmpty(custContact))
            {
                log.Info("custContact is null.");
            }
            else
            {
                log.Info("Contact cust. String is -" + custContact);
                log.Info("Length of the Contact cust. string is -" + custContact.Length);
                if (custContact.Contains("+") || custContact.Contains("(")
                    || custContact.Contains(")") || custContact.Contains("-"))
                {
                    formattedContact = removeSpecialCharacters(custContact);
                }
                else
                {
                    formattedContact = custContact;
                }
                if (formattedContact.Length > 10)
                {
                    formattedContact = formatContactLength(formattedContact);
                }
            }

            return formattedContact;
        }

        static string removeSpecialCharacters(string str)
        {
            log.Debug("Inside removeSpecialCharacters function.");
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(str))
            {
                log.Info("String is null.");
            }
            else
            {
                log.Info("Length of the string is -" + str.Length);
                log.Info("String is -" + str);
                
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] >= '0' && str[i] <= '9')
                    {
                        sb.Append(str[i]);
                    }
                }
            }
            return sb.ToString();
        }

        static string formatContactLength(string str)
        {
            log.Debug("Inside formatContactLength function.");
            string formattedContact = null;
            if (string.IsNullOrEmpty(str))
            {
                log.Info("String is null.");
            }
            else
            {
                log.Info("Length of the contact string is -" + str.Length);
                log.Info("String is -" + str);
                formattedContact = str;
                if (!string.IsNullOrEmpty(str) && str.Length > 10)
                {
                    formattedContact = str.Substring((str.Length - 10), 10);
                }
            }
            
            return formattedContact;
        }

        static int updateBaanTable(List<IRestResponse<RootObject>> objList)
        {
            IRestResponse<RootObject> response = null;
            int rowsAffectedCount = -1;
            int countOfIgnoredRecords = 0;
            // IRestResponse<RootObject> responseFromList = null;

            log.Debug("Inside updateBaanTable function.");
            int status = -1;

            log.Debug("Count of objectList is - " + objList.Count);
            for (int index = 0; index < objList.Count; index++)
            {
                response = objList[index];
                log.Info("Referring object from the list - " + index + " - out of " + objList.Count);

                if (response != null)
                {
                    //testing the list of response
                    var calls = response.Data.calls;
                    var callCount = calls.Count;
                    log.Info("Count of call data list is - " + callCount + " - at index - " + index);

                    if (calls == null || callCount == 0)
                    {
                        log.Debug("The call list in the response object is null, try again!");
                        // status is not updated
                    }
                    else
                    {
                        log.Debug("Inside else of null call list.");

                        // Create a connection to Baan and creating a Command using the connection
                        log.Info("Creating Connection to the string - " + Properties.Settings.Default.BaanConnectionString);
                        using (var conn = new System.Data.SqlClient.SqlConnection(Properties.Settings.Default.BaanConnectionString))
                        using (var command = conn.CreateCommand())
                        {
                            conn.Open();
                            log.Debug("Opened Connection.");

                            //Creating a command to check for the duplicates records in Baan table
                            var com = conn.CreateCommand();
                            log.Debug("Command to check for duplicate baan data and ignore them.");

                            // Start a local transaction.
                            SqlTransaction transaction = conn.BeginTransaction();
                            log.Debug("I am here 1.");

                            // variable to keep count of the query execution lines
                            int rowsAffected = 0;

                            // Must assign both transaction object and connection
                            // to Command object for a local transaction
                            command.Connection = conn;
                            com.Connection = conn;
                            command.Transaction = transaction;
                            com.Transaction = transaction;

                            // Create sql query text
                            command.CommandText = "INSERT INTO " + baanTableName + "  (t_cname, t_recid, t_cnum, t_ccity, " +
                                    "t_cstate, t_ccoun, t_calldt, t_mstat, t_ordat, t_mtdat, " +
                                    "t_orno, t_comp, t_oramnt, t_cuno, t_indat, t_tror, t_czip, " +
                                    "t_dept, t_keywrd, t_mdsrc, t_source, t_region,t_Refcntd, t_Refcntu) " +
                                    "VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, " +
                                    "@param8, @param9, @param10, @param11, @param12, @param13, @param14, @param15, @param16, " +
                                    "@param17, @param18, @param19, @param20, @param21, @param22, 0, 0)";

                            log.Debug("I am here 2.");

                            //insert into ttccom906260([t_recid], [t_cname], [t_cnum], [t_ccity], [t_cstate], [t_ccoun], 
                            //[t_calldt], [t_mstat], [t_ordat], [t_mtdat],
                            //[t_orno], [t_comp], [t_oramnt], [t_Refcntd], [t_Refcntu]) select(@param1, @param2, @param3,
                            //@param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11, @param12, @param13, 0, 0)"
                            //where not exists(select 'anything' from ttccom906260 where t_recid = '165598157')


                            command.Parameters.Add("@param1", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param2", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param3", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param4", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param5", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param6", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param7", SqlDbType.Date);
                            command.Parameters.Add("@param8", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param9", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param10", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param11", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param12", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param13", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param14", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param15", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param16", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param17", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param18", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param19", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param20", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param21", SqlDbType.VarChar, 50);
                            command.Parameters.Add("@param22", SqlDbType.VarChar, 10);

                            log.Debug("I am here 3.");

                            //checking for duplicate record ID values in Baan table
                            com.CommandText = "SELECT t_recid FROM " + baanTableName + " WHERE t_recid =" + "@param";
                            com.Parameters.Add("@param", SqlDbType.VarChar, 50);

                            log.Debug("I am here 4.");

                            try
                            {
                                // loop through the call list to get individual tag values
                                log.Debug("I am here 5.");
                                
                                //for (int callTagIndex = 1; callTagIndex < callCount; callTagIndex++)\
                                for (int callTagIndex = 0; callTagIndex < callCount; callTagIndex++)
                                {
                                    string cname = "";
                                    string recid = "";
                                    string cnum = "";
                                    string ccity = "";
                                    string cstate = "";
                                    string ccoun = "";
                                    string defaultDate = "1753/01/01";
                                    DateTime calldt = Convert.ToDateTime(defaultDate);     //DateTime.Now;     // setting the default to today's date
                                    int mstat = 0;
                                    string ordat = "";
                                    string mtdat = "";
                                    int orno = 0;
                                    int comp = -1;
                                    string locRegion = ""; 
                                    string oramnt = "";
                                    string cuno = "";
                                    string invoiceDate = "";
                                    string transactionOrigin = "";
                                    string formattedCnum = "";
                                    string mediaSource = "";
                                    string source = "CallRail";
                                    string keyword = "";
                                    int? callDuration = 0;

                                    callDuration = calls[callTagIndex].duration;
                                    if (callDuration < 60)
                                    {
                                        log.Debug("callDuration for index - " + callTagIndex + " is - "+ callDuration + " - which is less than 60 secs , for the response index - " + index);
                                        // ignore the records with less than 60 sec call duration
                                        countOfIgnoredRecords++;
                                        continue;
                                    }
                                    else
                                    {
                                        log.Debug("callDuration for index - " + callTagIndex + " is - " + callDuration + " - which is  greater than 60 secs , for the response index - " + index);
                                        cnum = calls[callTagIndex].customer_phone_number;
                                        if (cnum == null || cnum == "")
                                        {
                                            log.Debug("Cust number could not be fetched for index - " + callTagIndex + ", so moving on to next record!" + " , for the response index - " + index);
                                            log.Debug("Cust number is mandatory for Baan record match!");

                                        }
                                        else
                                        {
                                            formattedCnum = formatContact(cnum);
                                            recid = calls[callTagIndex].id;
                                            if (recid == null || recid == "")
                                            {
                                                log.Debug("record id could not be fetched for index - " + callTagIndex + ", so moving on to next record!" + " , for the response index - " + index);
                                            }
                                            else
                                            {
                                                //cnum = response.Data.calls[callTagIndex].customer_phone_number;
                                                cname = calls[callTagIndex].customer_name;
                                                if (cname == null)
                                                {
                                                    cname = "";
                                                    log.Debug("Cust Number could not be fetched for index - " + callTagIndex + " , for the response index - " + index);
                                                }
                                                else
                                                {
                                                    // the customer number cannot be found, 
                                                    // cannot search in Baan database without contact number
                                                    // have to put everything else in the else block or 
                                                    // replace the cname with cnum in the 1st if block
                                                }
                                                ccity = calls[callTagIndex].customer_city;
                                                if (ccity == null)
                                                {
                                                    ccity = "";
                                                    log.Debug("Cust City could not be fetched for index - " + callTagIndex + " , for the response index - " + index);
                                                }
                                                cstate = calls[callTagIndex].customer_state;
                                                if (cstate == null)
                                                {
                                                    cstate = "";
                                                    log.Debug("Cust State could not be fetched for index - " + callTagIndex + " , for the response index - " + index);
                                                }
                                                ccoun = calls[callTagIndex].customer_country;
                                                if (ccoun == null)
                                                {
                                                    ccoun = "";
                                                    log.Debug("Cust Country could not be fetched for index - " + callTagIndex + " , for the response index - " + index);
                                                }
                                                string myDate = calls[callTagIndex].start_time;
                                                if (myDate == null)
                                                {
                                                    myDate = "";
                                                    log.Debug("call date could not be fetched for index - " + callTagIndex + " , for the response index - " + index);
                                                }
                                                else
                                                {
                                                    // String to just Date conversion and excluding the time
                                                    /*int i = myDate.IndexOf("T");
                                                    if (i > 0)
                                                    {
                                                        myDate = myDate.Substring(0, i);
                                                    }*/
                                                    // calldt = DateTime.Parse(myDate);
                                                    // date and time format => 2019 - 02 - 08T14: 02:54.076 - 05:00
                                                    string calldate = getCallDate(myDate);
                                                    if (calldate == null || calldate == "")
                                                    {
                                                        calldate = "";
                                                        log.Debug("calldate for index - " + callTagIndex + " is null" + " , for the response index - " + index);
                                                    }
                                                    //calldt.ToShortDateString();
                                                    else
                                                    {
                                                        //calldt = DateTime.Parse(calldate);
                                                        calldt = DateTime.ParseExact(calldate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                                        //log.Debug("calldate set for index - " + callTagIndex + " is - " + calldt.Value.ToShortDateString() + " , for the response index - " + index);
                                                        log.Debug("calldate set for index - " + callTagIndex + " is - " + calldt.ToShortDateString() + " , for the response index - " + index);
                                                        
                                                    }
                                                    
                                                }

                                                log.Debug("I am here 14.");
                                                // These values are updated in the Baan matching and then fetched from there
                                                mstat = 0;
                                                log.Debug("Setting default for match status for index - " + callTagIndex + " , for the response index - " + index);
                                                ordat = "";
                                                log.Debug("Setting default for order date for index - " + callTagIndex + " , for the response index - " + index);
                                                mtdat = "";
                                                log.Debug("Setting default for match date for index - " + callTagIndex + " , for the response index - " + index);
                                                orno = 0;
                                                log.Debug("Setting default for order# for index - " + callTagIndex + " , for the response index - " + index);
                                                //comp = -1;
                                                String compName = calls[callTagIndex].source_name;
                                                log.Debug("I am here 15.");
                                                if (compName == null)
                                                {
                                                    log.Debug("source_name could not be fetched for index - " + callTagIndex + " , for the response index - " + index);
                                                    // Since I know it is Atlanta for now
                                                    comp = 0;
                                                    mediaSource = "";
                                                    locRegion = "";
                                                }
                                                else
                                                {
                                                    comp = getCompanyNumberFromName(compName);
                                                    if (comp == -1)
                                                    {
                                                        // fetch company number from the function 
                                                        //'getCompanyNumberFromName' fails setting company number to default #266
                                                        // Since I know it is Atlanta for now
                                                        comp = 0;
                                                    }
                                                    else
                                                    {
                                                        log.Debug("Company # for index - " + callTagIndex + " is set to - " + comp + " , for the response index - " + index);
                                                    }
                                                    mediaSource = getmediaSourceFromName(compName);
                                                    if (mediaSource == null)
                                                    {
                                                        // fetch media Source from the function 
                                                        //'getmediaSourceFromName' fails setting mediaSource to default blank
                                                        mediaSource = "";
                                                    }
                                                    else
                                                    {
                                                        log.Debug("mediaSource for index - " + callTagIndex + " is set to - " + mediaSource + " , for the response index - " + index);
                                                    }
                                                    locRegion = getlocRegFromName(compName);
                                                    if (locRegion == null || locRegion == "")
                                                    {
                                                        // fetch media Source from the function 
                                                        //'getmediaSourceFromName' fails setting mediaSource to default blank
                                                        locRegion = "";
                                                    }
                                                    else
                                                    {
                                                        log.Debug("location/Region for index - " + callTagIndex + " is set to - " + locRegion + " , for the response index - " + index);
                                                    }

                                                }
                                                string unformattedkeyword = calls[callTagIndex].keywords;
                                                keyword = formatKeyword(unformattedkeyword);
                                                if (keyword == null)
                                                {
                                                    log.Debug("keyword for index - " + callTagIndex + " is null. Setting to blank for the response index - " + index);

                                                   keyword = "";
                                                }
                                                else
                                                {
                                                    log.Debug("keyword for index - " + callTagIndex + " is set to - " + keyword + " , for the response index - " + index);
                                                }

                                                log.Debug("I am here 8.");
                                                oramnt = "";
                                                log.Debug("Setting default for order amnt for index - " + callTagIndex);
                                                cuno = "";
                                                log.Debug("Setting default for Cust Number for index - " + callTagIndex);
                                                invoiceDate = "";
                                                log.Debug("Setting default for invoice Date for index - " + callTagIndex);
                                                transactionOrigin = "";
                                                log.Debug("Setting default for transactionOrigin for index - " + callTagIndex);
                                            }
                                        }
                                    }

                                    //command.Parameters.Add(new SqlParameter("@param1", cname));
                                    //command.Parameters.Add(new SqlParameter("@param2", recid));
                                    //command.Parameters.Add(new SqlParameter("@param3", cnum));
                                    //command.Parameters.Add(new SqlParameter("@param4", ccity));
                                    //command.Parameters.Add(new SqlParameter("@param5", cstate));
                                    //command.Parameters.Add(new SqlParameter("@param6", ccoun));
                                    //command.Parameters.Add(new SqlParameter("@param7", calldt));
                                    //command.Parameters.Add(new SqlParameter("@param8", mstat));
                                    //command.Parameters.Add(new SqlParameter("@param9", ordat));
                                    //command.Parameters.Add(new SqlParameter("@param10", mtdat));
                                    //command.Parameters.Add(new SqlParameter("@param11", orno));
                                    //command.Parameters.Add(new SqlParameter("@param12", comp));
                                    //command.Parameters.Add(new SqlParameter("@param13", oramnt));

                                    command.Parameters[0].Value = cname;
                                    command.Parameters[1].Value = recid;
                                    //command.Parameters[2].Value = cnum;
                                    command.Parameters[2].Value = formattedCnum;
                                    command.Parameters[3].Value = ccity;
                                    command.Parameters[4].Value = cstate;
                                    command.Parameters[5].Value = ccoun;
                                    // The call date and time, ignoring time and consider only date
                                    /*if (calldt.HasValue)
                                    {
                                        //var shortString = calldt.Value.ToShortDateString();
                                        command.Parameters[6].Value = calldt.Value.ToShortDateString();
                                    }
                                    else
                                    {
                                        // Setting the default date as = 1/1/0001 12:00:00 AM
                                        calldt = new DateTime();
                                        command.Parameters[6].Value = calldt.Value.ToShortDateString();
                                    }*/
                                    command.Parameters[6].Value = calldt.ToShortDateString();
                                    command.Parameters[7].Value = mstat;        // To be filled after matching happens in Baan
                                    command.Parameters[8].Value = ordat;        // To be filled in Baan
                                    command.Parameters[9].Value = mtdat;        // To be filled in Baan
                                    command.Parameters[10].Value = orno;        // To be filled in Baan
                                    command.Parameters[11].Value = comp;
                                    command.Parameters[12].Value = oramnt;      // To be filled in Baan
                                    command.Parameters[13].Value = cuno;        // To be filled in Baan
                                    command.Parameters[14].Value = invoiceDate;     // To be filled in Baan
                                    command.Parameters[15].Value = transactionOrigin;
                                    command.Parameters[16].Value = "";         //postal zip, blank for now
                                    command.Parameters[17].Value = "";             // department code, to be filled from Baan
                                    command.Parameters[18].Value = keyword;         // keyword recorded while user called, keyword used to make the call
                                    command.Parameters[19].Value = mediaSource;     //This is the utm_source name, such as valpack or google, extracted value before underscore.
                                    command.Parameters[20].Value = source;      //This will be CallRail, since the source is CallRail, for others it will be email or Angie's list
                                    command.Parameters[21].Value = locRegion;      //This will be location/region attached to the name in CallRail account

                                    //checking for duplicate record ID values in Baan table
                                    com.Parameters[0].Value = recid;
                                    SqlDataReader reader = null;
                                    using (reader = com.ExecuteReader())
                                    {
                                        log.Debug("I am here 9.");
                                        //if (reader != null)
                                        if (reader.HasRows)
                                        {
                                            log.Debug("I am here 10.");
                                            // The record already exists so move to the next record
                                            Console.WriteLine("record ID - " + recid + " already exists, so moving on to next record.");
                                            continue;
                                        }
                                    }

                                    log.Info("INSERT INTO " + baanTableName + "  (t_cname, t_recid, t_cnum, t_ccity, " +
                                    "t_cstate, t_ccoun, t_calldt, t_mstat, t_ordat, t_mtdat, " +
                                    "t_orno, t_comp, t_oramnt, t_cuno, t_indat, t_tror, t_czip, " +
                                    "t_dept, t_keywrd, t_mdsrc, t_source, t_region, t_Refcntd, t_Refcntu) " +
                                    "VALUES ("+cname+","+ recid+","+ formattedCnum+","+ ccity+","+ cstate+","+ ccoun+","+ calldt+", " +
                                    mstat+","+ ordat+","+ mtdat+","+ orno+","+ comp+","+ oramnt+", "+cuno+", "+invoiceDate+"," +transactionOrigin+", " +
                                     " " + "," + " "+","+ keyword+","+ mediaSource+","+ source + "," + locRegion + ", 0, 0)");

                                    rowsAffected = command.ExecuteNonQuery();
                                    log.Debug("I am here 11.");
                                    log.Info(rowsAffected + " - Record written to Baan table CallRailDataFormatching, Table # " + baanTableName);
                                    rowsAffectedCount = rowsAffectedCount + 1;
                                    //disposing off the command for performance enhancement as well as clearing off for next loop cycle
                                    //command.Dispose();

                                    log.Info("Value of callTagIndex is -" + callTagIndex);

                                }
                                log.Debug("Ignored records count - " + countOfIgnoredRecords + ", for the response index - " + index);
                                // Attempt to commit the transaction.
                                transaction.Commit();
                                log.Debug("I am here 12.");
                                log.Debug("Committing the transaction.");
                                status = 0;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Exception Type: {0} " + ex.GetType());
                                log.Error("  Message: {0}" + ex.Message);

                                // Attempt to roll back the transaction.
                                try
                                {
                                    log.Error("Rolling back the transaction in case of error.");
                                    transaction.Rollback();
                                }
                                catch (Exception ex2)
                                {
                                    if (transaction.Connection != null)
                                    {
                                        // This catch block will handle any errors that may have occurred
                                        // on the server that would cause the rollback to fail, such as
                                        // a closed connection.
                                        log.Error("Rollback Exception Type: {0} " + ex2.GetType());
                                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                                        log.Error("  Message: {0} " + ex2.Message);
                                        Console.WriteLine("  Message: {0}", ex2.Message);
                                    }
                                    else
                                    {
                                        log.Error("Connection to the transaction is lost.");
                                        Console.WriteLine("Connection to the transaction is lost.");
                                    }
                                }
                            }
                            conn.Close();
                        }
                    }
                }
                else
                {
                    log.Error("Response is null from the response list at index - " + index);
                    Console.WriteLine("Response is null from the response list at index - " + index);
                }

            }
            log.Info("Total records ignored since duration was less - " + countOfIgnoredRecords); 
            //Instead of status, return the count of records updated in the table
            return rowsAffectedCount;
        }
    }
}