﻿using System.ComponentModel.DataAnnotations;

namespace ExploreCalifornia.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress, MaxLength(500)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

}