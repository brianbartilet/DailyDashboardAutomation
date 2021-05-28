using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using AppReferences.Utilities;
using System.Reflection;

namespace AppReferences
{
    public abstract class BaseRestApi
    {

        protected abstract Uri BaseAddress { get; }
        protected string AuthValue { get; set; }
        protected string SessionId { get; set; }
        protected static RestClient Client;
        protected RestRequest Request;

        public string Controller { get; set; }

        public IRestResponse Response;

        protected BaseRestApi()
        {
        }

        /// <summary>
        /// This is used to authorize and authenticate user.
        /// </summary>
        /// <returns>RestClient</returns>
        protected virtual RestClient Authorize()
        {
            var client = InitializeClient();

            return client;
        }

        /// <summary>
        /// This is used to initialize RestClient.
        /// </summary>
        /// <param name="initializeCookie">bool</param>
        /// <returns>RestClient</returns>
        protected virtual RestClient InitializeClient(bool initializeCookie = false)
        {
            var client = new RestClient(BaseAddress);
            if (initializeCookie)
            {
                client.CookieContainer = new CookieContainer();

                if (!string.IsNullOrEmpty(AuthValue))
                {
                    var cookie = AddCookie(".ASPXAUTH", AuthValue);
                    client.CookieContainer.Add(cookie);
                }

                if (!string.IsNullOrEmpty(SessionId))
                {
                    var cookie = AddCookie("ASP.NET_SessionId", SessionId);
                    client.CookieContainer.Add(cookie);
                }
            }
            
            return client;
        }

        /// <summary>
        /// This is used to initialize Rest Request.
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="controller">string</param>
        /// <param name="action">string</param>        
        /// <returns>RestRequest</returns>
        protected virtual RestRequest InitializeRequest(Method method, string controller, string action=null, object dto=null)
        {
            var request = new RestRequest
            {
                Method = method,
                RequestFormat = DataFormat.Json,
                Resource = controller + "/" + action,
                RootElement = "Data"
            };

            if (action == null)
            {
                request.Resource = controller;
            }
            //attach query parameters
            if (dto != null)
            {
                var fields = new List<FieldInfo>(dto.GetType().GetFields());
                foreach (var field in fields)
                {
                    var value = field.GetValue(dto);
                    if (value != null)
                    {
                        var name = char.ToLowerInvariant(field.Name[0]) + field.Name.Substring(1);
                        var useValue = value.ToString();

                        if (value.GetType() == typeof(bool))
                        {   if ((bool)value) useValue = useValue.ToLower();

                            else continue;             
                            
                        }

                        request.AddQueryParameter(name, useValue);
                    }
                }

            }

            return request;
        }

        /// <summary>
        /// This is used to add a cookie.
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="value">string</param>
        /// <returns></returns>
        protected Cookie AddCookie(string name, string value)
        {
            if (string.IsNullOrEmpty(ReadConfigFile.GetSettingAsString("BasePath")))
            {
                throw new Exception("Please add BasePath in AppSettings for the domain");
            }

            var cookie = new Cookie
            {
                Name = name,
                Value = value,
                Path = "/",
                Domain = ReadConfigFile.GetSettingAsString("BasePath")
            };

            return cookie;
        }
    }
}

