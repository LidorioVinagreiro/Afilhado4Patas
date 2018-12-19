using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Afilhado4Patas.Areas.Identity.Services;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Afilhado4Patas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly EmailSender _emailSender;
        private readonly RazorView _razorView;
        private readonly ApplicationDbContext _context;

        public ForgotPasswordModel(UserManager<Utilizadores> userManager, EmailSender emailSender, ApplicationDbContext context, RazorView razorView)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
            _razorView = razorView;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string display = "none";

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            display = "none";
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {                    
                     display = "block";
                }
                else
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { code },
                        protocol: Request.Scheme);
                    
                    string link = callbackUrl.ToString();
                    var perfil = _context.PerfilTable.Find(user.PerfilId);
                    var forgotPasswordModel = new EmailViewModel(link, perfil.FirstName);
                    string body = await _razorView.RenderViewToStringAsync("/Views/Emails/ChangePassword/ChangePassword.cshtml", forgotPasswordModel);
                    await _emailSender.SendEmailAsync(Input.Email, "Alterar Password", body);

                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
            }

            return Page();
        }
    }
}
