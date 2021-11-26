using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class SpotemonController : ControllerBase
{
    public static string clientId = "cf8b781360874ee4b651e982d0f361e2";
    public static string redirectUri = "https://localhost:7161/spotemon/login";
    public static string url5 = "https://accounts.spotify.com/api/token";
    public static string clientSecret = "CHANGE ME";

    [HttpGet("hello")]
    public IActionResult Get()
    {
        return Ok(clientId);
    }
    
    [HttpGet("login")]
    public SpotifyToken GetLogin() 
    {
        SpotifyToken token = new SpotifyToken();

        var encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientId, clientSecret)));

        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url5);

        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.Accept = "application/json";
        webRequest.Headers.Add("Authorization: Basic " + encode_clientid_clientsecret);

        var request = ("grant_type=client_credentials");
        byte[] req_bytes = Encoding.ASCII.GetBytes(request);
        webRequest.ContentLength = req_bytes.Length;

        Stream strm = webRequest.GetRequestStream();
        strm.Write(req_bytes, 0, req_bytes.Length);
        strm.Close();

        HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
        String json = "";
        using (Stream respStr = resp.GetResponseStream())
        {
            using (StreamReader rdr = new StreamReader(respStr, Encoding.UTF8))
            {
                //should get back a string i can then turn to json and parse for accesstoken
                json = rdr.ReadToEnd();
                rdr.Close();
            }
        }
        return token;
    }
}

