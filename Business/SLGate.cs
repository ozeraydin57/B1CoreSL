using B1CoreSL.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace B1CoreSL.Business
{
    public class SLGate
    {
        public LoginResp LoginSAP(LoginRequest login)
        {
            var LoginInfo = new LoginResp();
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                CookieContainer cookies = new CookieContainer();
                clientHandler.CookieContainer = cookies;

                var client = new HttpClient(clientHandler);
                var request = new HttpRequestMessage(HttpMethod.Post, $"{login.Host}/Login");
                var bodyParam = JsonConvert.SerializeObject(login);

                request.Content = new StringContent(bodyParam, null, "application/json");
                var response = client.Send(request);

                var respBody = response.Content.ReadAsStringAsync().Result;
                LoginInfo = JsonConvert.DeserializeObject<LoginResp>(respBody);

                if (LoginInfo.Error != null)
                    return LoginInfo;

                Uri uri = new Uri($"{login.Host}/Login");
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                var route = responseCookies.Where(k => k.Name == "ROUTEID");
                if (route != null)
                    LoginInfo.RouteId = route.FirstOrDefault().Value;

                LoginInfo.Host = login.Host;
                LoginInfo.CompanyDB = login.CompanyDB;
                LoginInfo.Password = login.Password;
                LoginInfo.UserName = login.UserName;
            }
            catch (Exception ex)
            {
                LoginInfo.Error = new Entity.Error() { message = ex.Message };
                LoginInfo.Error.message = ex.ToString();
            }

            return LoginInfo ?? new LoginResp();
        }
        public Response<string> LogoutSAP(LoginResp loginSapInfo)
        {
            string serviceLayerUrl = loginSapInfo.Host + $"/Logout";

            var rs = SLRequest(loginSapInfo, serviceLayerUrl, HttpMethod.Post, null);

            return rs;
        }

        public Response<string> SLRequest(LoginResp loginSapInfo, string serviceLayerUrl, HttpMethod method, object obj)
        {
            var resp = new Response<string>();

            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                var client = new HttpClient(clientHandler);
                var request = new HttpRequestMessage(method, serviceLayerUrl);
                request.Headers.Add("Cookie", $"B1SESSION={loginSapInfo.SessionId}; ROUTEID={loginSapInfo.RouteId}");
                request.Headers.Add("Prefer", "odata.maxpagesize=1000");

                if (obj != null)
                {
                    var bodyParam = JsonConvert.SerializeObject(obj);
                    request.Content = new StringContent(bodyParam, null, "application/json");
                }

                var response = client.Send(request);

                var loginResp = new LoginResp();
                var respBody = response.Content.ReadAsStringAsync().Result;

                var err = JsonConvert.DeserializeObject<LoginResp>(respBody);

                if (err?.Error != null)
                {
                    resp.Success = false;
                    resp.Message = err.Error.message;
                }
                else
                    resp.Success = true;

                resp.Data = respBody;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.ToString();
            }


            return resp;
        }
        public List<Response<string>> AddUDF(LoginResp loginSapInfo, List<UDFEntity> udflist)
        {
            var resp = new List<Response<string>>();

            foreach (var udf in udflist)
            {
                string serviceLayerUrl = loginSapInfo.Host + "/UserFieldsMD";
                var rs = SLRequest(loginSapInfo, serviceLayerUrl, HttpMethod.Post, udf);
                resp.Add(rs);
            }

            return resp;
        }
        public Response<string> AddQuery(LoginResp login, string sqlName, string sql)
        {
            string serviceLayerUrl = login.Host + $"/SQLQueries('{sqlName}')";
            var rs = SLRequest(login, serviceLayerUrl, HttpMethod.Delete, null);

            serviceLayerUrl = login.Host + "/SQLQueries";
            rs = SLRequest(login, serviceLayerUrl, HttpMethod.Post, new { SqlCode = sqlName, SqlName = sqlName, SqlText = sql });

            return rs;
        }
        public object ViewExpose(LoginResp login)
        {
            string serviceLayerUrl = login.Host + "/SQLViews('*')/Unexpose";
            var rs = SLRequest(login, serviceLayerUrl, HttpMethod.Post, null);

            serviceLayerUrl = login.Host + "/SQLViews('*')/Expose";
            rs = SLRequest(login, serviceLayerUrl, HttpMethod.Post, null);
            return rs;
        }
    }
}