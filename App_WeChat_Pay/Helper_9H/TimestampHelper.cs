using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper_9H
{
    public class TimestampHelper
    {
        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateTimeStamp()
        {
             TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
             return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
