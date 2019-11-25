using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace versionamento.Models.ViewModels
{
    public class PullProjeto
    {
        public PullProjeto(string pull, string pullDate)
        {
            Pull = pull;
            PullDate = Convert.ToDateTime(pullDate);
        }

        public string Pull { get; set; }
        public DateTime PullDate { get; set; }
    }
}