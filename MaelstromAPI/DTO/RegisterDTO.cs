using System.ComponentModel.DataAnnotations;


namespace Maelstrom.API.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string? FirstName { get; set; }
        [Required] 
        public string? LastName { get; set}
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
