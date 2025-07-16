using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;


namespace AuthService.Application.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números e ter 11 dígitos.")]
        public string Cpf { get; set; } = null!;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "O campo Perfil é obrigatório.")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "O campo Cargo é obrigatório.")]
        public string Position { get; set; } = null!;
    }
}
