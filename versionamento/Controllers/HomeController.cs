using System.Web.Mvc;
using versionamento.Service;

namespace versionamento.Controllers
{
    public class HomeController : Controller
    {

       readonly Home serviceHome = new Home();

        public ActionResult Index()
        {
            return View(serviceHome.ListarRepositorios());
        }

    }
}