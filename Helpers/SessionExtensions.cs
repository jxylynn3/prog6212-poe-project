using Microsoft.AspNetCore.Http;

namespace ST10448420_CMCsystem.Helpers
{
    // the purpose of the seesion helper class is to  make the implementation of sesssion services easier ,by 
    // creating extension methods for usage in the controllers and views
    public static class SessionExtensions
    {
        public static string UserID(this ISession session)
            => session.GetString("UserID");

        public static string UserRole(this ISession session)
            => session.GetString("UserRole");

        public static string UserName(this ISession session)
            => session.GetString("UserName");
    }
}
