using System;

namespace Wezel_Sieciowy1
{
    class ProtokolControl
    {
        public const String LRM     = "lrm";            //przychodzi od LRM
        public const String LRM_R   = "lrm_resp";       //wysylane do LRM, że przyjąłem i git
        public const String CONN    = "connection_request"; //przychodzi od Gienerała, żadanie zestawienia połączenia
        public const String CONN_R  = "connection_resp";    //wysylane do Gienerała, że połączenie zestawione, pamietac o odeslaniu id polaczenia
        public const String CONN_F  = "connection_failed";  //wysylane do Gienerała, że niemożliwe zestawienie połączenia o id
        public const String DISCONN = "disconnect";         //przychodzi od Gienerała, aby rozłączyć połączenie o id
        public const String DISCONN_R = "disconnect_resp"; //jak sama nazwa wskazuje
        public const String DISCONN_F = "disconnect_failed";// j/w
        public const String LOGIN   = "login";              //wysylane do Gienerała, żeby zalogować
        public const String LOGIN_R = "confirmation";       //przychodzi od Gienerała, jeśli uda się zalogować
        public const String TOPOLOGY = "topology";          //wysyłane do Gienerała, o łączach na początku
        public const String BAD     = "bad_command";        //jak sama nazwa wskazuje, uniwersalne, LRM np tego uzywa
    }
}
