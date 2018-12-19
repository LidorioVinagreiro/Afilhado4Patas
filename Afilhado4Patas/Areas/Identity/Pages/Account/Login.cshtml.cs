using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Afilhado4Patas.Data;

namespace Afilhado4Patas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<Utilizadores> signInManager, UserManager<Utilizadores> userManager,ILogger<LoginModel> logger, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public string display = "none";

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

            [Display(Name = "Lembrar-se de mim?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            display = "none";
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var utilizador = _context.Utilizadores.Where(u => u.Email == Input.Email).FirstOrDefault();
                if (utilizador != null) {
                    if (utilizador.Active)
                    {
                        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");

                            Utilizadores user = _userManager.Users.Where(u => u.Email == Input.Email).FirstOrDefault();
                            IList<string> roles = await _userManager.GetRolesAsync(user);
                            string role = roles.FirstOrDefault();
                            if (role == "Utilizador")
                                return RedirectToAction("Index", "Utilizador");//LocalRedirect(returnUrl);
                            if (role == "Responsavel")
                                return RedirectToAction("Index", "Responsavel");//LocalRedirect(returnUrl);
                            if (role == "Funcionario")
                                return RedirectToAction("Index", "Funcionario");//LocalRedirect(returnUrl);

                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Tentativa de Inicio de Sessão Falhada");
                            return Page();
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Esta conta encontra-se bloqueada. Tente novamente com outra conta!");
                        return RedirectToPage("./Lockout");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativa de Inicio de Sessão Falhada");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
