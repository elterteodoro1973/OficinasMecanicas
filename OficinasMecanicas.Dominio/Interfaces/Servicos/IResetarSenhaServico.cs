﻿using OficinasMecanicas.Dominio.Entidades;

namespace OficinasMecanicas.Dominio.Interfaces.Servicos
{
    public interface IResetarSenhaServico
    {    
        Task Adicionar(ResetarSenha resetarSenha);        
        Task Excluir(Guid usuarioId);
        Task<ResetarSenha?> BuscarResetarSenhaPorToken(string token);
    }
}
