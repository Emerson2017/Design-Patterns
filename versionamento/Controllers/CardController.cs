using System.Web.Mvc;
using versionamento.Service;

namespace versionamento.Controllers
{
    public class CardController : Controller
    {

       readonly Card serviceHome = new Card();

        public ActionResult Index()
        {
            return View(serviceHome.ListarRepositorios());
        }

    }
}