using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class MailKitEmailServiceOptions
    {
            public MailKitEmailServiceOptions()
            {
                HostSecureSocketOptions = SecureSocketOptions.Auto;
            }

            public SecureSocketOptions HostSecureSocketOptions { get; set; }

            public string HostAddress { get; set; }

            public int HostPort { get; set; }

            public string HostUsername { get; set; }

            public string HostPassword { get; set; }

            public string SenderEmail { get; set; }

            public string SenderName { get; set; }
        }
    }
