using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatchFilms.Models
{
    public class LoginUser
    {
        [Required (ErrorMessage = "Debe ingresar un usuario.")]
        public string User { get; set; }
        [Required(ErrorMessage = "Debe ingresar una contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}