using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinasMecanicas.Dominio.Entidades.Validacoes.Oficina
{
    
    public class CadastrarEditarOficinaValidacao : AbstractValidator<Entidades.OficinaMecanica>
    {

        public CadastrarEditarOficinaValidacao(bool validarIdentificador = false)
        {
            if (validarIdentificador)
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty).WithMessage("O campor identificador da oficiona não pode ser vazio");
            }

            RuleFor(c => c.Nome)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres")
                .EmailAddress().WithMessage("O campo {PropertyName} precisa ser um e-mail válido !");

            RuleFor(c => c.Endereco)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres");

            RuleFor(c => c.Servicos)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(512).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres");


        }
    }



}
