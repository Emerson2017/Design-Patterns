//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace versionamento.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Repositorio
    {
        public int Id { get; set; }
        public int IdProjeto { get; set; }
        public string Codigo { get; set; }
        public Nullable<int> IdTipo { get; set; }
    
        public virtual Projeto Projeto { get; set; }
        public virtual Tipo Tipo { get; set; }
    }
}
