using SharpBucket.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using versionamento.Models;

namespace versionamento.Service
{
    public class Home
    {
        private CatalogoDeProdutosEntities dbx = new CatalogoDeProdutosEntities();
        private const string codUserAPI = "arcoeducaosistemas";

        public List<Repositorios_VM> ListarRepositorios()
        {

            List<Repositorios_VM> ListaVM = new List<Repositorios_VM>();

            try
            {
                var projetos = dbx.Repositorio.Select(x=> x.Projeto).Distinct().ToList();
                
                Repositorios_VM objRepositorio;
                List<PullProjeto> ListPull;


               


                foreach (var objProjeto in projetos)
                {
                    objRepositorio = new Repositorios_VM();
                    ListPull = new List<PullProjeto>();
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


                    objRepositorio.PullRequest = (ListPull.Count > 0 ? CommonMark.CommonMarkConverter.Convert(ListPull.OrderByDescending(x => x.PullDate).SingleOrDefault().Pull) : objRepositorio.PullRequest);
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

            var sharpBucket = new SharpBucketV2();
            sharpBucket.OAuth2ClientCredentials("6ewwFSyWdwCbCUmGBj", "u8WGYeHTWkjpbQTWPAessbCbMfuvdVjU");
            codRepositorio = objprojeto.Repositorio.Where(x => x.Tipo.Nome == tipo).SingleOrDefault();

            if (tipo == "APP")
            {
                repositorio.VersaoAPP = (sharpBucket.RepositoriesEndPoint().TagResource(codUserAPI, codRepositorio.Codigo).ListTags().OrderByDescending(x => x.date).Select(x => x.name).FirstOrDefault() ?? repositorio.VersaoAPP);

            }
            else
            {
                repositorio.VersaoAPI = (sharpBucket.RepositoriesEndPoint().TagResource(codUserAPI, codRepositorio.Codigo).ListTags().OrderByDescending(x => x.date).Select(x => x.name).FirstOrDefault() ?? repositorio.VersaoAPI);

            }

            var pullRequest = sharpBucket.RepositoriesEndPoint().PullRequestsResource("arcoeducaosistemas", codRepositorio.Codigo).GetPullRequestLog()
                    .Where(x => x.update != null && x.update.destination.branch.name == "master").OrderByDescending(x => x.update.date).FirstOrDefault();

            if (pullRequest != null && pullRequest.update != null && !string.IsNullOrEmpty(pullRequest.update.description))
            {
                ListPull.Add(new PullProjeto(pullRequest.update.description, pullRequest.update.date));
            }

        }



    }
}