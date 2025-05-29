using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Entidades.Validacoes.Agenda
{
    //internal class CadastrarEditarAgendaValidacao
    //{
    //}


    public class CadastrarEditarAgendaValidacao : AbstractValidator<Entidades.AgendamentoVisita>
    {
        public CadastrarEditarAgendaValidacao(bool validarIdentificador = false)
        {
            if (validarIdentificador)
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty).WithMessage("O campo identificador da oficina não pode ser vazio");
            }

            RuleFor(c => c.IdUsuario)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}");

            RuleFor(c => c.IdOficina)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(Guid.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}");

            RuleFor(c => c.Descricao)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres");

            RuleFor(c => c.DataHora)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .GreaterThan(DateTime.MinValue).WithMessage("O campo {PropertyName} precisa ser uma data válida");
        }
    }


}
