using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Afilhado4Patas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Utilizadores> _userManager;

        public ResetPasswordModel(UserManager<Utilizadores> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Preencha este campo com o seu Email!")]
            [EmailAddress(ErrorMessage = "Insira um email no formato exemplo@exemplo.com")]
            [StringLength(50, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required(ErrorMessage = "Preencha este campo com a sua Password!")]
            [PasswordAtLeast1LetterAnd1Number]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "A sua password deverá ter letras (aA) e pelo menos 1 numero e não deverá ter caracteres que não letras ou numeros")]
            [StringLength(15, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-Passe")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Preencha este campo com a sua Password!")]
            [PasswordAtLeast1LetterAnd1Number]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "A sua password deverá ter letras (aA) e pelo menos 1 numero e não deverá ter caracteres que não letras ou numeros")]
            [StringLength(15, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-Passe")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = code
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
