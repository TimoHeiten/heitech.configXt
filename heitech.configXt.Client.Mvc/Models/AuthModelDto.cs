using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace heitech.configXt.Client.Mvc
{
    public class AuthModelDto
    {
        [Required]
        [EmailAddress]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [HiddenInput]
        public string From { get; set; }
    }
}