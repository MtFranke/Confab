using System;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Agendas.Application.Submissions.Events.External
{
    internal record SpeakerCreated(Guid Id, string FullName) : IEvent;
}