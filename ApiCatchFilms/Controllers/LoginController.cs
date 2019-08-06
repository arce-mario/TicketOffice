using ApiCatchFilms.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiCatchFilms.Controllers
{
    public class LoginController : ApiController
    {
        // POST: api/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoginAsync(LoginUser usuarioLogin)
        {
            if (usuarioLogin == null)
                return BadRequest("Usuario y Contraseña requeridos.");

            var _userInfo = await AutenticarUsuarioAsync("", "");
            if (_userInfo != null)
            {
                return Ok(new { token = GenerarTokenJWT(_userInfo) });
            }
            else
            {
                return Unauthorized();
            }
        }
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();
        // COMPROBAMOS SI EL USUARIO EXISTE EN LA BASE DE DATOS 
        private async Task<User> AutenticarUsuarioAsync(string userName, string password)
        {
            IQueryable<User> result = db.Users.Where(u => u.firstName.Equals(userName) && u.pass.Equals(password));
            User user = await db.Users.FindAsync(result.First().userID);
            // AQUÍ LA LÓGICA DE AUTENTICACIÓN //

            // Supondremos que el usuario existe en la Base de Datos.
            // Retornamos un objeto del tipo UsuarioInfo, con toda
            // la información del usuario necesaria para el Token.
            return user;

            // Supondremos que el usuario NO existe en la Base de Datos.
            // Retornamos NULL.
            //return null;
        }

        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(User usuarioInfo)
        {
            string admin = "Admin";
            string publicuser = "public";
            string rol = "";
            if (usuarioInfo.userID == 1)
            {
                rol = admin;
            }
            else
            {
                rol = publicuser;
            }

            // RECUPERAMOS LAS VARIABLES DE CONFIGURACIÓN
            var _ClaveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
            var _Issuer = ConfigurationManager.AppSettings["Issuer"];
            var _Audience = ConfigurationManager.AppSettings["Audience"];
            if (!Int32.TryParse(ConfigurationManager.AppSettings["Expires"], out int _Expires))
                _Expires = 24;


            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_ClaveSecreta));
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.userID.ToString()),
                new Claim("nombre", usuarioInfo.firstName),
                new Claim("apellidos", usuarioInfo.lastName),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.email),
                new Claim(ClaimTypes.Role, rol)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _Issuer,
                    audience: _Audience,
                    claims: _Claims,
                    notBefore: DateTime.Now,
                    // Exipra a la 24 horas.
                    expires: DateTime.Now.AddHours(_Expires)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}
