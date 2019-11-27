using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace versionamento.Models
{
    public class Repositorios_VM
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string VersaoAPP { get; set; }
        public string VersaoAPI { get; set; }
        public string Descricao { get; set; }
        public string PullRequest { get; set; }
        public List<Unidade> Unidade { get; set; }
    }
}