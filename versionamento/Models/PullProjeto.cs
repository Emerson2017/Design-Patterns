using System;

namespace versionamento.Models
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