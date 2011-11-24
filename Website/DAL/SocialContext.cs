using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Website.Models;

namespace Website.DAL
{
  public class SocialContext : DbContext
  {
    public DbSet<Enrollment> Enrollments { get; set; }
  }
}