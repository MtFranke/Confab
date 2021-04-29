using System;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Domain.Submissions.Entities
{
    public class Speaker : AggregateRoot
    {
        public string FullName { get; set; }

        public Speaker(AggregateId id,string fullName)
        {
            FullName = fullName;
            Id = id;
        }

        public static Speaker Create(Guid id, string fullName)
            => new Speaker(id, fullName);
    }
}