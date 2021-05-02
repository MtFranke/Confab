using System;
using System.Collections.Generic;

namespace Confab.Modules.Agendas.Application.Submissions.DTO
{
    public class SubmissionDto
    {
        public Guid Id { get; set; }
        public Guid ConferenceId { get; set; }
        public Guid Title { get; set; }
        public Guid Description { get; set; }
        public int Level { get; set; }
        public string Status { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<SpeakerDto> Speakers { get; set; }
    }
}