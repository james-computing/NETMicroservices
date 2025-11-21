using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public class CommandReadDTO
    {
        public int Id { get; set; }
        public required string HowTo { get; set; }
        public required string CommandLine { get; set; }
        public int PlatformId { get; set; }

    }
}
