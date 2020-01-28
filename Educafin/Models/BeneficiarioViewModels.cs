//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;

//namespace Educafin.Models
//{
//    public class RegistroViewModel
//    {
//        [Required]
//        [Display(Name = "Correo")]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [Display(Name = "Confirmar Correo")]
//        [EmailAddress]
//        public string EmailV { get; set; }

//        [Required]
//        [DataType(DataType.Password)]
//        [Display(Name = "Password")]
//        public string Password { get; set; }

//        [Display(Name = "Remember me?")]
//        public bool RememberMe { get; set; }
//    }
//}