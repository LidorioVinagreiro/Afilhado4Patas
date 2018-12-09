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
using Afilhado4Patas.Views.Emails.ConfirmAccount;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly EmailSender _emailSender;
        private readonly ApplicationDbContext _contexto;
        private readonly RazorView _razorView;

        public RegisterModel(
            UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager,
            ILogger<RegisterModel> logger,
            EmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext contexto,
            RazorView razorView)
        {
            _roleManager = roleManager;
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
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public string Nome { get; set; }

            public string Apelido { get; set; }

            public DateTime DataNascimento { get; set; }
            
            public string Genero { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
                var user = new Utilizadores { UserName = Input.Email, Email = Input.Email, PerfilId = null };
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
                    var x = _roleManager.RoleExistsAsync("Utilizador");
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
                    string link = HtmlEncoder.Default.Encode(callbackUrl);
                    var confirmAccountModel = new ConfirmAccountEmailViewModel(link);                   

                    string body = await _razorView.RenderViewToStringAsync("/Views/Emails/ConfirmAccount/ConfirmAccount.cshtml", confirmAccountModel);
                    
                    await _emailSender.SendEmailAsync(Input.Email, "Confirme o seu email", $"<a href=\"{link}\">carrega</a>");

                    // await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
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
