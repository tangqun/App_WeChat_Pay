using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model_9H
{
    [Serializable]
    public partial class CodeMsgModel
    {
        private int _code;
        private string _msg;

        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
    }
}
