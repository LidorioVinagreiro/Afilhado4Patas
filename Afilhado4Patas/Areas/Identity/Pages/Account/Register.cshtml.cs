using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Afilhado4Patas.Areas.Identity.Services;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Afilhado4Patas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailSender _emailSender;
        private readonly ApplicationDbContext _contexto;
        private readonly RazorView _razorView;

        public RegisterModel(
            UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager,
            ILogger<RegisterModel> logger,
            EmailSender emailSender,
            ApplicationDbContext contexto,
            RazorView razorView)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _contexto = contexto;
            _razorView = razorView;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Preencha este campo com o seu Email!")]
            [EmailAddress(ErrorMessage = "Insira um email no formato exemplo@exemplo.com")]
            [StringLength(50, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Preencha este campo com o seu Nome!")]
            [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
            [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "Preencha este campo com o(s) seu(s) Apelido(s)!")]
            [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
            [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
            public string Apelido { get; set; }

            [Display(Name = "Data de Nascimento")]
            [Required(ErrorMessage = "Preencha este campo com a sua Data de Nascimento!")]
            [DateGreatThen18]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime DataNascimento { get; set; }

            [Required(ErrorMessage = "Selecione um dos campos Masculino ou Feminino")]
            public string Genero { get; set; }

            [Required(ErrorMessage = "Preencha este campo com a sua Password!")]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "A sua password deverá ter letras (aA) e pelo menos 1 numero e não deverá ter caracteres que não letras ou numeros")]
            [StringLength(15, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [PasswordAtLeast1LetterAnd1Number]
            [Display(Name = "Palavra-Passe")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Preencha este campo com a sua Password!")]
            [DataType(DataType.Password)]
            [PasswordAtLeast1LetterAnd1Number]
            [Display(Name = "Confirme Palavra-Passe")]
            [Compare("Password", ErrorMessage = "As palavras-passes inseridas não são iguais")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new Utilizadores { UserName = Input.Email, Email = Input.Email, Active = true};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    Perfil perfilUtilizador = new Perfil
                    {
                        UtilizadorId = user.Id,
                        FirstName = Input.Nome,
                        LastName = Input.Apelido,
                        Genre = Input.Genero,
                        Birthday = Input.DataNascimento
                    };
                    _contexto.PerfilTable.Add(perfilUtilizador);
                    _contexto.SaveChanges();
                    user.PerfilId = perfilUtilizador.Id;
                    _contexto.SaveChanges();

                    IdentityResult resultRole = await _userManager.AddToRoleAsync(user, "Utilizador");
                    if (resultRole.Succeeded)
                    {
                        _logger.LogInformation("Atribuido Role");
                    }
                    else {
                        _logger.LogInformation("Falha no role");
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    string link = callbackUrl.ToString();
                    var confirmAccountModel = new EmailViewModel(link, Input.Nome);
                    string body = await _razorView.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccount.cshtml", confirmAccountModel);
                    await _emailSender.SendEmailAsync(Input.Email, "Confirme o seu email", body);

                    return RedirectToAction("RegistoCompleto", "Guest");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }        
    }
}
