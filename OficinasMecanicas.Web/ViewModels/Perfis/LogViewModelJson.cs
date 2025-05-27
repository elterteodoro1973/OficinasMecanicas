using System;
using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Web.ViewModels.Perfis
{
	public class LogViewModelJson
    {
		public LogViewModelJson()
		{
            data = new HashSet<LogPerfilJsonViewModel>();
        }

        public virtual ICollection<LogPerfilJsonViewModel>? data { get; set; }
    }
}

