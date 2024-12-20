// ViewModels/UserViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace Shop_Maintain.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public byte? LoaiUser { get; set; }
    }
}
