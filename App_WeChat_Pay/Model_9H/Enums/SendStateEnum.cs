using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_9H.Enums
{
    public enum SendStateEnum
    {
        未发放 = 1,
        发放中 = 2,
        已发放待领取 = 3,
        发放失败 = 4,
        已领取 = 5,
        已退款 = 6,
    }
}
