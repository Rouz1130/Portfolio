using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class Project
    {
        public string Name { get; set; }
        public string Starred { get; set; }

        public static List<Project> GetProjects()
        {
            var client = new RestClient("https://api.github.com/users/rouz1130/repos");
            var request = new RestRequest("Accounts/" + EnvironmentVariables.AccountSid + "/Projects.json", Method.GET);
            
            request.AddParameter("access_token" ,"ded0c0da5407eb87e1f5ff7964b47079f10a6d3b");
            request.AddHeader("User-Agent", "rouz1130");
            request.AddHeader("Accept", "application/vnd.github.v3+json");


            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();


            JObject jsonResponse = JsonConvert.DesrializeObject > (response.Content);
            var projectList = JsonConvert.DeserializeObject<List<Project>>(jsonResponse["projects"].ToString());
            return projectList;
        }
        public static Task<IRestResponse> GetResponseContentAsynch(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
            
      }

     }
  }

