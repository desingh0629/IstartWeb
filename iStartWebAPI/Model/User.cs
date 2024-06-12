using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iStartWebAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Password { get; set; }
       
    }
}
