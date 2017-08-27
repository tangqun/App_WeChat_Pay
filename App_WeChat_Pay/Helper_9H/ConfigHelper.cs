using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper_9H
{
    public class ConfigHelper
    {
        public static string ConnStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
    }
}
