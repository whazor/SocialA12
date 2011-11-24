using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Models
{
  public class FacebookUser
  {
    public long FacebookId { get; set; }
    public string AccessToken { get; set; }
    public DateTime Expires { get; set; }

    public string Name { get; set; }
  }
}
