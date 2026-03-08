using ApplicationLayer.ViewModels;
using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AdministrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AdministrationService(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<List<User>> ReadAll()
        {
            return _userManager.Users.OrderBy(u => u.Name).ToList();
        }

        public async Task<User?> Read(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> SearchByParameter(string? parameter)
        {
            var users = _userManager.Users.ToList();

            if (string.IsNullOrWhiteSpace(parameter))
                return users.OrderBy(u => u.Name).ToList();

            parameter = parameter.Trim().ToLower();

            return users
                .Where(u =>
                    (!string.IsNullOrWhiteSpace(u.Name) && u.Name.ToLower().Contains(parameter)) ||
                    (!string.IsNullOrWhiteSpace(u.Email) && u.Email.ToLower().Contains(parameter)))
                .OrderBy(u => u.Name)
                .ToList();
        }

        public async Task<IdentityResult> Create(UserViewModel model, string resetPasswordPageUrl)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Има потребител с този имейл."
                });
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true
            };

            var tempPassword = $"Tmp!{Guid.NewGuid():N}aA1";

            var result = await _userManager.CreateAsync(user, tempPassword);

            if (!result.Succeeded)
                return result;

            // ✅ добавяне в роля User
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return roleResult;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = $"{resetPasswordPageUrl}?code={code}&email={Uri.EscapeDataString(user.Email!)}";

            await _emailSender.SendEmailAsync(
                user.Email!,
                "Създаден акаунт",
                $"Вашият акаунт е създаден. Натиснете върху линка, за да зададете парола: " +
                $"<a href='{callbackUrl}'>Създай парола</a>");

            return result;
        }

        public async Task<IdentityResult> Update(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id!);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Потребителят не е намерен."
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Има потребител с този имейл."
                });
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Потребителят не е намерен."
                });
            }

            return await _userManager.DeleteAsync(user);
        }
    }

}

