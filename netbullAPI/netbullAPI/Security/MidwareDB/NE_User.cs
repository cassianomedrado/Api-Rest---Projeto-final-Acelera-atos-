using netbullAPI.Interfaces;
using netbullAPI.Security.Models;
using netbullAPI.Security.Persistencia;
using netbullAPI.Security.ViewModels;
using netbullAPI.Util;
using System.Data;

namespace netbullAPI.Security.MidwareDB
{
    public class NE_User
    {
        private UserDAO _userDao;

        public NE_User(UserDAO userDao)
        {
            _userDao = userDao;
        }

        public async Task <List<RetornarUserViewModel>> getAllUsersAsync()
        {
            return await _userDao.getAllUsersAsync();
        }

        internal async Task<User> CadastroDeUserAsync(User usu)
        {
            usu.user_accessKey = Criptografia.HashValue(usu.user_accessKey);

            usu = await _userDao.CadastroDeUserAsync(usu);

            return usu;
        }

        public async Task<User> RecuperarUsuarioAsync(User usu)
        {
            usu =  await _userDao.RecuperarUsuarioAsync(usu);
            return usu;
        }

        public async Task<User> VerificarUsuarioSenhaAsync(User usu)
        {
            usu.user_accessKey = Criptografia.HashValue(usu.user_accessKey);

            var verificado = await _userDao.VerificarUsuarioSenhaAsync(usu);
            
            return verificado;
        }

        internal async Task<bool> DeleteUserAsync(int id)
        {
            return await _userDao.DeleteUserAsync(id);
        }

        internal async Task<bool> alterarSenhaAsync(User usu)
        {
            usu.user_accessKey = Criptografia.HashValue(usu.user_accessKey);
            return await _userDao.alterarSenhaAsync(usu);
        }
    }
}
