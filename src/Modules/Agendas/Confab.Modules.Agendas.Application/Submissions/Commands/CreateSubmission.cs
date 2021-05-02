using System;
using System.Collections.Generic;
using Confab.Shared.Abstractions.Commands;

namespace Confab.Modules.Agendas.Application.Submissions.Commands
{
    public class CreateSubmission : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid ConferenceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Guid> SpeakersIds { get; set; }

        public CreateSubmission(Guid conferenceId, string title, string description, int level,
            IEnumerable<string> tags, IEnumerable<Guid> speakersIds)
        {
            ConferenceId = conferenceId;
            Title = title;
            Description = description;
            Level = level;
            Tags = tags;
            SpeakersIds = speakersIds;
        }
    }
}