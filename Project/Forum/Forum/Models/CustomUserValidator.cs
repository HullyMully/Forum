using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class CustomUserValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.Email.ToLower().EndsWith("@spam.com"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Данный домен находится в спам-базе. Выберите другой почтовый сервис!"
                });
            }
            if (user.UserName.ToLower().Contains("admin"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Ник пользователя не должен содержать слово 'admin'!"
                });
            }
            if(manager.Users.Where(x => x.UserName.ToLower() == user.UserName.ToLower()).Count() != 0)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Аккаунт с ником '{user.UserName}' уже сущесвтует!"
                });
            }
            if (manager.Users.Where(x => x.Email.ToLower() == user.Email.ToLower()).Count() != 0)
            {
                errors.Add(new IdentityError
                {
                    Description = $"Данный email уже привязан к другому аккаунту!"
                });
            }

            string symbols = "._-abcdefghijklmnopqrstuvwxyz1234567890";
            for (int i = 0; i < user.UserName.Length; i++)
                for (int j = 0; j < symbols.Length; j++)
                    if (user.UserName.ToLower()[i] != symbols[j])
                    {
                        if (j == symbols.Length - 1)
                        {
                            errors.Add(new IdentityError
                            {
                                Description = $"В никe есть недопустимые символы!"
                            });
                            i = symbols.Length + 1;
                            break;
                        }     
                    }
                    else break; 

            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
