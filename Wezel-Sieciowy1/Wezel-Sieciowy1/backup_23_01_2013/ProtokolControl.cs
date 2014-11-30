using System;

namespace Wezel_Sieciowy1
{
    class ProtokolControl
    {
        public const String LRM     = "lrm";            //przychodzi od LRM
        public const String LRM_R   = "lrm_resp";       //wysylane do LRM, że przyjąłem i git
        public const String CONN    = "connect";        //przychodzi od Gienerała
        public const String CONN_R  = "connect_resp";   //wysylane do Gienerała, że połączenie zestawione, pamietac o odeslaniu id polaczenia
        public const String CONN_F  = "connect_failed"; //wysylane do Gienerała, że niemożliwe zestawienie połączenia o id
        public const String LOGIN   = "login";          //wysylane do Gienerała, żeby zalogować
        public const String LOGIN_R = "login_resp";     //przychodzi od Gienerała, jeśli uda się zalogować
    }
}
