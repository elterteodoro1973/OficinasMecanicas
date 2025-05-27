
using FluentValidation;

namespace OficinasMecanicas.Dominio.Entidades.Validacoes.Usuarios
{
    public class CadastrarEditarUsuarioValidacao : AbstractValidator<Entidades.Usuarios>
    {

        public CadastrarEditarUsuarioValidacao(bool validarIdentificador = false)
        {
            if (validarIdentificador)
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty).WithMessage("O campor identificador de usuário não pode ser vazio");
            }

            RuleFor(c => c.Email)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres")
                .EmailAddress().WithMessage("O campo {PropertyName} precisa ser um e-mail válido !");

            RuleFor(c => c.Username)
                .NotNull().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .NotEqual(string.Empty).WithMessage("O campo {PropertyName} não pode ser igual a {ComparisonValue}")
                .MaximumLength(256).WithMessage("O campo {PropertyName} não pode ser maior que {MaxLength} caracteres");

           

        }
    }
}
