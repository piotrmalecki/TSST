using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client1
{

    public class Protocol
    {

        public const String LOGIN = "login";

        public const String END = "end";

        public const String PARAMETERS = "parameters";

        public const String CONFIRMATION = "confirmation";

        public const String CONNECTION = "connection";

        public const String SET = "set";

        public const String SET_RSP = "set_rsp";

        public const String CLOSE = "close";

        public const String NULLCOMMAND = "nullcommand";

        public const String ALIVE = "alive";

        public const String CALL_IND = "call_indication";
        public const String CALL_ACCEPT = "call_accept";
        public const String CALL_TEAR = "call_teardown";
        public const String CALL_FAIL = "call_failed";
    }

}
