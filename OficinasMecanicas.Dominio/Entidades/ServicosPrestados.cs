﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OficinasMecanicas.Dominio.Entidades;

public partial class ServicosPrestados
{
    [Key]
    public int Id { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Nome { get; set; }
}
