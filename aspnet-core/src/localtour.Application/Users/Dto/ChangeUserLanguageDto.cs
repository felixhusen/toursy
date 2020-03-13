using System.ComponentModel.DataAnnotations;

namespace localtour.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}