using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class Project
    {
        public string name { get; set; }
        public string stargazers_count { get; set; }
      
        

        public static List<Project> GetProjects()
        {

            var client = new RestClient("https://api.github.com/search/repositories?page=1&q=user:rouz1130&sort=stars:>1&order=desc");
            var request = new RestRequest(/*"Accounts/" + EnvironmentVariables.AccountSid + "/Projects.json",*/ Method.GET);
            
            request.AddParameter("Access_token","EnvironmentVariables.AccessToken");
            request.AddHeader("User-Agent", "rouz1130");
            // .star not sure if needs to be there after v3, might just have to do with more information time of project.
            request.AddHeader("Accept", "application/vnd.github.v3.text-match+json");

            client.Authenticator = new HttpBasicAuthenticator("/Itmes.json", EnvironmentVariables.AccessToken); 

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();


            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            // might not need []// took it out fixed error. could not serialize the ["projects"]
            var projectList = JsonConvert.DeserializeObject<List<Project>>(jsonResponse["items"].ToString());
            return projectList;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
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

