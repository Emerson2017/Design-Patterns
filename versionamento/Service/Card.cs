using SharpBucket.V2;
using SharpBucket.V2.EndPoints;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using versionamento.Models;

namespace versionamento.Service
{
    public class Card
    {
        private CatalogoDeProdutosEntities dbx = new CatalogoDeProdutosEntities();
        private string codUserAPI = ConfigurationManager.AppSettings["teamKey"];

        public List<Repositorios_VM> ListarRepositorios()
        {

            List<Repositorios_VM> ListaVM = new List<Repositorios_VM>();

            try
            {
                var projetos = dbx.Repositorio.Select(x=> x.Projeto).Distinct().ToList();
                
                Repositorios_VM objRepositorio;
                List<PullProjeto> ListPull;



                ListPull = new List<PullProjeto>();


                foreach (var objProjeto in projetos)
                {
                    objRepositorio = new Repositorios_VM();
                    objRepositorio.VersaoAPP = "v0.0";
                    objRepositorio.VersaoAPI = "v0.0";
                    objRepositorio.PullRequest = "Versão não encontrada";
                    objRepositorio.Id = objProjeto.Id;

                    if (objProjeto.Repositorio.Any(x=> x.Tipo.Nome == "APP")) {

                        PullRequest(ref objRepositorio, ref ListPull, "APP", objProjeto);

                    }

                    if (objProjeto.Repositorio.Any(x => x.Tipo.Nome == "API"))
                    {
                        PullRequest(ref objRepositorio, ref ListPull, "API", objProjeto);

                    }

                    DateTime date = ListPull.Max(x => x.PullDate);
                    objRepositorio.PullRequest = (ListPull.Count > 0 ? CommonMark.CommonMarkConverter.Convert(ListPull.Where(y => y.PullDate == date).SingleOrDefault().Pull) : objRepositorio.PullRequest);
                    objRepositorio.Nome = objProjeto.Cabecalho;
                    objRepositorio.Descricao = objProjeto.Descricao;

                    ListaVM.Add(objRepositorio);
                }

            }
            catch (Exception ex)
            {

            }

                return ListaVM;
        }



        private void PullRequest(ref Repositorios_VM repositorio, ref List<PullProjeto> ListPull, string tipo, Projeto objprojeto)
        {
            Repositorio codRepositorio;

            Repositorio objeto_repositorio;
            objeto_repositorio = objprojeto.Repositorio.Where(x => x.Tipo.Nome == tipo).SingleOrDefault();
            
            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials(ConfigurationManager.AppSettings["consumerKey"], ConfigurationManager.AppSettings["consumerSecretKey"]);
            codRepositorio = objprojeto.Repositorio.Where(x => x.Tipo.Nome == tipo).SingleOrDefault();

            if (tipo == "APP")
            {
                repositorio.VersaoAPP = (sharpBucket.RepositoriesEndPoint().TagResource(codUserAPI, codRepositorio.Codigo).ListTags().OrderByDescending(x => x.date).Select(x => x.name).FirstOrDefault() ?? repositorio.VersaoAPP);

            }
            else
            {
                repositorio.VersaoAPI = (sharpBucket.RepositoriesEndPoint().TagResource(codUserAPI, codRepositorio.Codigo).ListTags().OrderByDescending(x => x.date).Select(x => x.name).FirstOrDefault() ?? repositorio.VersaoAPI);

            }

            ListParameters parameters = new ListParameters
            {
                Filter = $"destination.branch.name = \"master\"",
                Sort = "-updated_on",
                Max = 1
            };

            var pullRequest = sharpBucket.RepositoriesEndPoint().PullRequestsResource(codUserAPI, objeto_repositorio.Codigo).ListPullRequests(parameters).FirstOrDefault();


            if (pullRequest != null && !string.IsNullOrEmpty(pullRequest.title))
                ListPull.Add(new PullProjeto(pullRequest.title, pullRequest.created_on));

        }



    }
}