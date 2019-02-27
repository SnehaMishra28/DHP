using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using RestSharp;

namespace UpdateCallRailData
{
    class Program
    {
        private const string apiKey = "620ddf5b5f5860ccd0ce7976054b86bd";
        private const string baanTableName = "ttccom906260";
        private static ILog log = LogManager.GetLogger(typeof(Program));


        static void Main(string[] args)
        {
            try
            {
                log.Info("Inside Main for UpdateCallRailData.");
                Console.WriteLine("Inside Main for UpdateCallRailData.");

                List<RecordOrderDetails> baanRecords = getBaanTableData();
                //if (baanRecords != null || baanRecords.Count>0)
                if (baanRecords != null )
                {
                    log.Info("Data fetched from Baan Table - " + baanTableName);
                    // loop through the data and update call rail
                    int callRailUpdateStatus = updateCallRail(baanRecords);
                    if (callRailUpdateStatus > 0)
                    {
                        log.Info(callRailUpdateStatus + " - Records successfully updated in Call Rail account.");
                        Console.WriteLine(callRailUpdateStatus + " - Data successfully updated in Call Rail account.");
                    }
                    else
                    {
                        log.Info("Callrail Data update to the account failed.");
                        Console.WriteLine("Callrail Data update to the account failed.");
                    }
                }
                else
                {
                    log.Error("No data fetched from Baan Table - "+ baanTableName);
                    Console.WriteLine("No data fetched from Baan Table - " + baanTableName);
                }
            }
            catch (Exception e)
            {
                log.Error("Exception in Main program occured! " + e);
                Console.WriteLine("Exception in Main program occured! " + e);
            }
            finally
            {
#if DEBUG
                Console.WriteLine("In DEBUG - Press any key...");
                Console.ReadKey();
#endif
            }
        }
        
        static List<RecordOrderDetails> getBaanTableData()
        {
            log.Debug("Inside getBaanTableData function.");
            Console.WriteLine("Inside getBaanTableData function..");
            int status = -1;
            List<RecordOrderDetails> recOrder = new List<RecordOrderDetails>();

            // Create a connection to Baan 
            log.Info("Creating Connection to the string - " + Properties.Settings.Default.BaanConnectionString);
            using (var conn = new System.Data.SqlClient.SqlConnection(Properties.Settings.Default.BaanConnectionString))
            
            // creating a Command using the connection
            using (var command = conn.CreateCommand())
            {
                Console.WriteLine("Inside 1");
                conn.Open();
                log.Debug("Opened Connection.");
                Console.WriteLine("Inside 2");
                // Must assign both transaction object and connection
                // to Command object for a local transaction
                command.Connection = conn;

                // cretating sql query text
                command.CommandText = "SELECT t_recid, t_orno, t_oramnt from  " + baanTableName + " where t_orno > 0";
                Console.WriteLine("Inside 3");
                try
                {
                    Console.WriteLine("Inside 4");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Inside 5");
                        if (reader.HasRows)
                        {
                            //List<RecordOrderDetails> recOrder = new List<RecordOrderDetails>();
                            
                            Console.WriteLine("Inside 6");
                            while (reader.Read())
                            {
                                Console.WriteLine("Inside 7");
                                Console.WriteLine(reader.GetSqlValue(0) +" - "+ reader.GetSqlValue(1) +"  - " + reader.GetSqlValue(2));

                                //Adding the record details in the list
                                recOrder.Add(new RecordOrderDetails() { recid = reader.GetSqlValue(0).ToString(),
                                                                        orno = reader.GetSqlValue(1).ToString(),
                                                                        oramnt = reader.GetSqlValue(2).ToString() });
                            }
                            reader.NextResult();
                        }
                        

                        //var employees = RecordOrderDetails(true);
                        //foreach (var e in RecordOrderDetails)
                        //{
                        //    //Change #4 -- Write all fields to the response.
                        //    Response.Write("<span>{0}</span><span>{1}</span><span>{2}</span>", e.Name, e.code, e.Left);
                        //}

                        // The record already exists so move to the next record
                       // Console.WriteLine("select statement does not return null.");
                           // status = 0;
                       // }
                        else
                        {
                            Console.WriteLine("Inside 8");
                            Console.WriteLine("select statement returns null.");
                        }
                        reader.Close();
                        ((IDisposable)reader).Dispose();    //always good idea to do proper cleanup
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inside 9");
                    log.Error("Exception Type: {0} " + ex.GetType());
                    log.Error("  Message: {0}" + ex.Message);
                }
                conn.Close();
            }
            Console.WriteLine("Inside 10");
            return recOrder;
        }

        static int updateCallRail(List<RecordOrderDetails> passBaanRecords)
        {
            log.Debug("Inside updateCallRail function.");
            Console.WriteLine("Inside updateCallRail function");
            int putStatusCount = 0;
            //var client = new RestSharp.RestClient(Properties.Settings.Default.fetchNoteValueFieldFromCallRail);
            //var client = new RestSharp.RestClient("https://api.callrail.com/v2/a/638986454/calls/{callId}.json");
           // var client = new RestSharp.RestClient("https://api.callrail.com/v2/a/638986454/calls/{callId}.json?value={value}&note={note}");
            
            var request = new RestSharp.RestRequest(RestSharp.Method.PUT);
            request.AddHeader("Authorization", "Token token=" + apiKey);
            //loop through the hash map to get individual recod values and match status

            int recordCount = passBaanRecords.Count();
            for (int i=0; i< recordCount; i++)
            {
                Console.WriteLine("Inside 11");
                // call id = record ID
                string callID = passBaanRecords[i].recid;
                // note = order #
                string note = passBaanRecords[i].orno;
                //value is order amount
                string value = passBaanRecords[i].oramnt;


                //Adding the fields values in the statement
                var client = new RestSharp.RestClient("https://api.callrail.com/v2/a/638986454/calls/"+ callID + ".json?value="+value+"&note="+note); 

                //request.AddParameter("callId", callID, ParameterType.UrlSegment);
                //request.AddParameter("value", value, ParameterType.QueryString);
                //request.AddParameter("note", note, ParameterType.QueryString);

                IRestResponse response = client.Execute(request);

                if (response != null)
                {
                    Console.WriteLine("Inside 12");
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Unauthorized:
                        case HttpStatusCode.NotFound:
                            log.Info("Inside case NotFound!");
                            log.Error("Response status code is - " + response.StatusCode);
                            Console.WriteLine("Response status code is - " + response.StatusCode);
                            break;
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Created:
                        case HttpStatusCode.Accepted:
                            log.Info("Inside case OK!");
                            Console.WriteLine("Inside 13");
                            log.Info("Response here status code is - " + response.StatusCode);

                            log.Debug("successful PUT for index - " + i);
                            Console.WriteLine("successful PUT for index - " + i);
                            putStatusCount = putStatusCount+1;
                            break;
                        case HttpStatusCode.InternalServerError:
                            log.Info("Inside case InternalServerError!");
                            log.Info("Response status code is - " + response.StatusCode);
                            Console.WriteLine("Response status code is -" + response.StatusCode);
                            break;
                        default:
                            //Nothing since something went wrong, just display error message to user
                            log.Info("Inside case Default!");
                            log.Error("Response status code is - " + response.StatusCode);
                            Console.WriteLine("Default - Response status code is -" + response.StatusCode);
                            break;
                    }

                }
                else
                {
                    log.Debug("Unsuccessful response fetched for index - " + i);
                    Console.WriteLine("Unsuccessful response fetched for index - " + i);
                }
            }
            Console.WriteLine(putStatusCount + " - successful PUT out of - " + recordCount);
            return putStatusCount;
        }
    }
}
