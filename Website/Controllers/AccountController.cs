using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using Website.Models;

namespace Website.Controllers
{
  [Authorize]
  public class AccountController : Controller
  {
    private const string logoffUrl = "http://localhost:1041/";
    private const string redirectUrl = "http://localhost:1041/Account/OAuth";
    //
    // GET: /Account/LogOn
    [AllowAnonymous]
    public ActionResult LogOn(string returnUrl)
    {
      var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
      oAuthClient.RedirectUri = new Uri(redirectUrl);

      var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> {
        { "display", "popup" },
        { "state", returnUrl },
        { "scope", "email" },
        { "expires", 3 }
      });
      return Redirect(loginUri.AbsoluteUri);
    }

    [AllowAnonymous]
    public ActionResult OAuth(string code, string state)
    {
      FacebookOAuthResult oauthResult;
      if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult) && oauthResult.IsSuccess)
      {
        var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
        oAuthClient.RedirectUri = new Uri(redirectUrl);
        dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
        string accessToken = tokenResult.access_token;

        DateTime expiresOn = DateTime.MaxValue;

        if (tokenResult.ContainsKey("expires"))
          DateTimeConvertor.FromUnixTime(tokenResult.expires);

        FacebookClient fbClient = new FacebookClient(accessToken);
        dynamic me = fbClient.Get("me?fields=id,name");
        long facebookId = Convert.ToInt64(me.id);

        InMemoryUserStore.Add(new FacebookUser
        {
          AccessToken = accessToken,
          Expires = expiresOn,
          FacebookId = facebookId,
          Name = (string)me.name,
        });

        FormsAuthentication.SetAuthCookie(facebookId.ToString(), false);

        // prevent open redirection attack by checking if the url is local.
        if (Url.IsLocalUrl(state))
        {
          return Redirect(state);
        }
      }
      return RedirectToAction("Index", "Home");
    }

    //
    // GET: /Account/LogOff

    public ActionResult LogOff()
    {
      FormsAuthentication.SignOut();
      var oAuthClient = new FacebookOAuthClient()
      {
        RedirectUri = new Uri(logoffUrl)
      };
      var logoutUrl = oAuthClient.RedirectUri;
      return Redirect(logoutUrl.AbsoluteUri);
    }
  }
}
