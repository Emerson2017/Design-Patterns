using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using versionamento.Models;
using versionamento.Models.ViewModels;

namespace versionamento.Controllers
{
    public class HomeController : Controller
    {

        private CatalogoDeProjetosEntities dbx = new CatalogoDeProjetosEntities();
        private const string codUserAPI = "arcoeducaosistemas";

        public ActionResult Index()
        {

            List<RepositoriosVM> ListaVM = new List<RepositoriosVM>();

            try
            {
                var projetos = dbx.Repositorio.Select(x => x.Projeto).Distinct().ToList();

                RepositoriosVM objRepositorio;
                List<PullProjeto> ListPull;


                foreach (var objProjeto in projetos)
                {


                   var teste = objProjeto.Repositorio.Any(x => x.Tipo.Nome == "APP");



                    objRepositorio = new RepositoriosVM();
                    ListPull = new List<PullProjeto>();
                    objRepositorio.VersaoAPP = "v0.0";
                    objRepositorio.VersaoAPI = "v0.0";
                    objRepositorio.PullRequest = "Versão não encontrada";
                    objRepositorio.Id = objProjeto.Id;


                   
                    

                    if (objProjeto.Repositorio.Any(x => x.Tipo.Nome == "APP"))
                    {
                        Repositorio codRepositorio;
                        Repositorio objeto_repositorio;

                        objeto_repositorio = objProjeto.Repositorio.Where(x => x.Tipo.Nome == "APP").SingleOrDefault();

                        var sharpBucket = new SharpBucketV2();
                        sharpBucket.OAuth2ClientCredentials("6ewwFSyWdwCbCUmGBj", "u8WGYeHTWkjpbQTWPAessbCbMfuvdVjU");
                        codRepositorio = objProjeto.Repositorio.Where(x => x.Tipo.Nome == "APP").SingleOrDefault();

                        objRepositorio.VersaoAPP = (sharpBucket.RepositoriesEndPoint().TagResource(codUserAPI, codRepositorio.Codigo).ListTags().OrderByDescending(x => x.date).Select(x => x.name).FirstOrDefault() ?? objRepositorio.VersaoAPP);

                        ListParameters parameters = new ListParameters
                        {
                            Filter = $"destination.branch.name = \"master\"",
                            Sort = "-updated_on",
                            Max = 1
                        };

                        var pullRequest = sharpBucket.RepositoriesEndPoint().PullRequestsResource(codUserAPI, objeto_repositorio.Codigo).ListPullRequests(parameters).FirstOrDefault();

                    }

                    if (objProjeto.Repositorio.Any(x => x.Tipo.Nome == "API"))
                    {
                        Repositorio codRepositorio;
                        Repositorio objeto_repositorio;

                        objeto_repositorio = dbx.Repositorio.Where(x => x.Tipo.Nome == "API").SingleOrDefault();

                        var sharpBucket = new SharpBucketV2();
                        sharpBucket.OAuth2ClientCredentials("6ewwFSyWdwCbCUmGBj", "u8WGYeHTWkjpbQTWPAessbCbMfuvdVjU");
                        codRepositorio = dbx.Repositorio.Where(x => x.Tipo.Nome == "API").SingleOrDefault();

                        objRepositorio.VersaoAPP = (sharpBucket.RepositoriesEndPoint().TagResource(codUserAPI, codRepositorio.Codigo).ListTags().OrderByDescending(x => x.date).Select(x => x.name).FirstOrDefault() ?? objRepositorio.VersaoAPP);

                        ListParameters parameters = new ListParameters
                        {
                            Filter = $"destination.branch.name = \"master\"",
                            Sort = "-updated_on",
                            Max = 1
                        };

                        var pullRequest = sharpBucket.RepositoriesEndPoint().PullRequestsResource(codUserAPI, objeto_repositorio.Codigo).ListPullRequests(parameters).FirstOrDefault();

                    }


                    objRepositorio.PullRequest = (ListPull.Count > 0 ? CommonMark.CommonMarkConverter.Convert(ListPull.OrderByDescending(x => x.PullDate).SingleOrDefault().Pull) : objRepositorio.PullRequest);
                    objRepositorio.Nome = objProjeto.Cabecalho;
                    objRepositorio.Descricao = objProjeto.Descricao;

                    ListaVM.Add(objRepositorio);
                }

            }
            catch (Exception ex)
            {
                return View(ListaVM);
            }


            return View(ListaVM);

        }

    }
}
