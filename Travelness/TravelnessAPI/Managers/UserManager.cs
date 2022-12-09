using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelnessAPI.Models.User;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Managers
{
    public class UserManager
    {
        private SHA256 hasher;
        private readonly IUnitOfWork unitOfWork;
        private readonly EmailManager emailManager;
        private readonly IMapper mapper;

        private const string PassSalt = "123456789101";
        private const string EmailSalt = "234567891011";

        public UserManager(IUnitOfWork uow, EmailManager emailManager, IMapper mapper)
        {
            hasher = SHA256.Create();
            this.unitOfWork = uow;
            this.emailManager = emailManager;
            this.mapper = mapper;
        }

        public void Register(UriBuilder callback, UserRegisterViewModel model)
        {
            var user = unitOfWork.Users.Get(x => x.Email == model.Email).FirstOrDefault();
            if (user != null && user.IsConfirmed)
            {
                throw new ArgumentException("User email is already registered.");
            }           

            if (user == null)
            {
                user = unitOfWork.Users.Get(x => x.Username == model.Username).FirstOrDefault();
                if (user != null)
                {
                    throw new ArgumentException("This username is already in use.");
                }
                else
                {
                    user = mapper.Map<User>(model);
                    user.PasswordHash = ComputeHash(model.Password + PassSalt);
                    //user = new User { Email = model.Email, Username = model.Username, Role = model.Role, PasswordHash = ComputeHash(model.Password + PassSalt) };
                    unitOfWork.Users.Insert(user);
                }               
            }
            else
            {
                user = mapper.Map<User>(model);
                user.PasswordHash = ComputeHash(model.Password + PassSalt);
                //user = new User { Email = model.Email, Username = model.Username, Role = model.Role, PasswordHash = ComputeHash(model.Password + PassSalt) };
                unitOfWork.Users.Update(user);
            }
            unitOfWork.Commit();

            callback.Query = $"username={user.Username}&token={ComputeHash(user.Email + EmailSalt)}";
            var message = "Для завершения регистрации перейдите по ссылке: <a href=\""
                          + callback.Uri + "\">завершить регистрацию</a>";

            emailManager.SendEmail(model.Email, "Confirm your account", message);
        }

        private string ComputeHash(string data)
        {
            var bytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private bool VerifyHash(string password, string hash) => hash == ComputeHash(password);

        public User CheckPassword(string email, string password)
        {
            var user = unitOfWork.Users.Get(x => x.Email == email && x.IsConfirmed == true).FirstOrDefault();
            if (user != null)
            {
                return VerifyHash(password + PassSalt, user.PasswordHash) ? user : null;
            }

            return null;
        }

        public bool IsOriginToken(string email, string token)
        {
            return VerifyHash(email + EmailSalt, token) ? true : false;
        }
    }
}
