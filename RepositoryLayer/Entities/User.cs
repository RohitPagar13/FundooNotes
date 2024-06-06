using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RepositoryLayer.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [StringLength(50)]
        [MinLength(2, ErrorMessage = "Please Enter Valid First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }


        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Phone number")]
        [RegularExpression(@"^\+?\d{1,3}?\d{10}$", ErrorMessage = "Enter a valid Phone number")]
        [StringLength(14, ErrorMessage = "Phone number cannot be longer than 14 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [MinLength(8, ErrorMessage = "Enter password with a minimum length of 8 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$", ErrorMessage = "Enter a valid Password")]
        public string Password { get; set; }

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/[0-9]{4}$", ErrorMessage = "Enter valid date")]
        [Required(ErrorMessage = "Please Enter date")]
        [StringLength(10)]
        public string BirthDate { get; set; }
    }
}
