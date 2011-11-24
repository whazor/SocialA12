using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Website.Models;

namespace Website.DAL
{
  public class SocialInitializer : DropCreateDatabaseIfModelChanges<SocialContext>
  {

  }
}