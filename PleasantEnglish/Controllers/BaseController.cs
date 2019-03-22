﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
    public class BaseController : Controller
    {
	    private ApplicationUserManager _userManager;
	    internal SiteContext db = new SiteContext();

	    public class ReCaptchaClass
	    {
		    public static string Validate(string EncodedResponse)
		    {
			    var client = new System.Net.WebClient();

			    string PrivateKey = WebConfigurationManager.AppSettings["reCAPTCHASecret"];

			    var GoogleReply = client.DownloadString(string.Format(
				    "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

			    var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

			    return captchaResponse.Success.ToLower();
		    }

		    [JsonProperty("success")]
		    public string Success
		    {
			    get { return m_Success; }
			    set { m_Success = value; }
		    }

		    private string m_Success;

		    [JsonProperty("error-codes")]
		    public List<string> ErrorCodes
		    {
			    get { return m_ErrorCodes; }
			    set { m_ErrorCodes = value; }
		    }


		    private List<string> m_ErrorCodes;
	    }

	}
}