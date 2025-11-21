using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public class CommandCreateDTO
    {
        [Required]
        public required string HowTo { get; set; }

        [Required]
        public required string CommandLine { get; set; }
    }
}
