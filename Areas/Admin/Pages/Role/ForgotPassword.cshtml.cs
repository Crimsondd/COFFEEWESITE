﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DACS_DAMH.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Azure.Core;
using System.Security.Policy;

namespace DACS_DAMH.Areas.Admin.Pages.Role
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public ApplicationUser user { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {

            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Không có user, id = {id}.");
            }


            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string id)
        {

            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Không có user, id = {id}.");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Role/ResetPassword",
                pageHandler: null,
                values: new { area = "Admin", code },
                protocol: Request.Scheme);

            await MailUtils.MailUtils.SendMailGoogleSmtp("nguyenthanhnhan1000pro@gmail.com", user.Email, "Reset Password",
                   $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                                         "nguyenthanhnhan1000pro@gmail.com", "yqyn mmyx zjyw hqiu");


            return RedirectToPage("./ForgotPasswordConfirmation");

        }
    }
}
