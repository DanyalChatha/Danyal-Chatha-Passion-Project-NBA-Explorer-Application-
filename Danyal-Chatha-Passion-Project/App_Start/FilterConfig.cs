using System.Web;
using System.Web.Mvc;

namespace Danyal_Chatha_Passion_Project
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
