﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Web.ViewModels.Perfis
{
	public class LogViewModelDetalhesJson
	{
        public string Id { get; set; }
        public string id { get; set; }
        public string Nome { get; set; } = null!;       
        public string? Descricao { get; set; }
        public bool Excluido { get; set; }

        

    }
}

