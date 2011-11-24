using System;
using System.Collections.Generic;

namespace Website.Models
{
  public class Enrollment
  {
    public int EnrollmentID { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }

    public string School { get; set; }

    // Too big int
    public string FacebookID { get; set; }

  }
}