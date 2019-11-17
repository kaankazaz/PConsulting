using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PConsulting.Data.BOL
{
    // Added for extending the default IdentityUser class for possible future requests - Kaan KAZAZ
    public class ApplicationUser : IdentityUser
    {

    }
}
