using AutoMapper;
using Marquise_Web.Data;
using Marquise_Web.Data.IRepository;
using Marquise_Web.Service.IService;
using Marquise_Web.Utilities.convert;
using MArquise_Web.Model.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Marquise_Web.Service.Service
{
    public class CRMAccountService : ICRMAccountService
    {
        private readonly ICRMAccountRepository repository;
        private readonly IMapper mapper;

        public CRMAccountService(ICRMAccountRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        

        public async Task<UserModel> LoginAsync(string username, string password)
        {
            var user = await repository.GetUserByUserNameAsync(username);
            if (user == null) return null;
            if (await PasswordHelper.VerifyPasswordAsync(password, user.Password))
            {
                user.LastLogin = DateTime.Now;
                await repository.AddUser(user); 
                var userModel = mapper.Map<UserModel>(user);
                return userModel;
            }
            return null;
        }

        public async Task<bool> UpdatePasswordAsync(UserModel userModel)
        {
            var user = await repository.GetUserByUserNameAsync(userModel.UserName);
            if (user == null) return false;
            var password = await PasswordHelper.HashPasswordAsync(userModel.Password);
            return await repository.UpdatePasswordAsync(user, password);
        }



    }
}
