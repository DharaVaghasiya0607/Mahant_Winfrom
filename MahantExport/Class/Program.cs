﻿using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MahantExport.Class
{

    public  class Program
    {
        public const string DEFAULT_REPORT_NUMBER = "1206489210";
        public const string GRAPHQL_QUERY_FILE = "../graphql_query/report_results.graphql";

        public static void CertiTest(string StrReportNo)
        {
            // Get parameters from environmental variables. Do not store secrets in code!
            string url = "https://api.reportresults.gia.edu";
            string key = "ec3fba70-349c-4dc3-ac4b-1c4f22cf10f0";

            // Confirm that url and key are available
            if (string.IsNullOrEmpty(url) | string.IsNullOrEmpty(key))
            {
                Console.WriteLine("You must provide environment variables REPORT_RESULTS_API_ENDPOINT and REPORT_RESULTS_API_KEY.");
                System.Environment.Exit(1);
            }

            // Load the query from a file
            string query = "";
            try
            {
                query = File.ReadAllText(GRAPHQL_QUERY_FILE);
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                System.Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            // Set the report number to lookup
            string reportNumber;
            if (StrReportNo.Length == 0)
            {
                reportNumber = DEFAULT_REPORT_NUMBER;
            }
            else
            {
                reportNumber = StrReportNo;
            }
            Console.WriteLine("Looking up report number: " + reportNumber + "\n");

            // Construct the payload to be POSTed to the graphql server
            var query_variables = new Dictionary<string, string> {
                { "ReportNumber", reportNumber}
            };
            var payload = new Dictionary<string, object> {
                { "query", query },
                { "variables", query_variables }
            };

            // Pretty-print the JSON for readability

            // Convert the payload to JSON
            string json = JsonConvert.SerializeObject(payload);

            // Write the payload to the console
            Console.WriteLine("JSON PAYLOAD TO BE POSTED TO THE SERVER");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(json + "\n");

            // The results will be saved in this dictionary
            Dictionary<string, string> reportResults = new Dictionary<string, string>();

            using (var client = new WebClient())
            {
                // Set headers for the api key and content-type
                client.Headers.Add(HttpRequestHeader.Authorization, key);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                string response = "";
                try
                {
                    // Send the payload as a JSON to the endpoint
                    response = client.UploadString(url, json);
                }
                catch (System.Net.WebException e)
                {
                    Console.Write("Error accessing " + url + ": ");
                    Console.WriteLine(e.Message);
                    System.Environment.Exit(1);
                }

                Console.WriteLine("JSON RESPONSE RECEIVED FROM THE API");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine(response + "\n");

                // Parse the response (a string) into a JsonDocument so we can
                // traverse the fields

                // Check for errors in the response
                if (reportResults.ContainsKey("/errors/0/message"))
                {
                    Console.Write("Error processing request: ");
                    Console.WriteLine(reportResults["/errors/0/message"] + "\n");
                }

                // Write all data to the console
                Console.WriteLine("PARSED REPORT RESULTS");
                Console.WriteLine("---------------------");
                foreach (KeyValuePair<string, string> entry in reportResults)
                {
                    Console.WriteLine(entry.Key + ": " + entry.Value);
                }
            }
        }
    }
}