using System;
using System.ComponentModel.DataAnnotations;

namespace Confab.Modules.Conferences.Core.DTO
{
    internal class ConferenceDto
    {
        public Guid Id { get; set; }
        [Required] public Guid HostId { get; set; }
        public Guid Host { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Location { get; set; }

        public string LogoUrl { get; set; }
        public int? ParticipantLimit { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}