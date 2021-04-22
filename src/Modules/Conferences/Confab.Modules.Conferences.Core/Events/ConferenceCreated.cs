using System;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Conferences.Core.Events
{
    public record ConferenceCreated(Guid Id, string Name, int? ParticipantLimit, DateTime From, DateTime To) : IEvent;
}