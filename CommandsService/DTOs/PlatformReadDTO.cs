using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public class PlatformReadDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
