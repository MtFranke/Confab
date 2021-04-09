using System;
using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Conferences.Core.Exceptions
{
    internal class CannotDeleteHostException: ConfabException
    {
        public Guid Id { get; }

        public CannotDeleteHostException(Guid id) : base($"Host with ID: '{id}' cannot be deleted")
        {
            Id = id;
        }
    }
    
    internal class CannotDeleteConferenceException: ConfabException
    {
        public Guid Id { get; }

        public CannotDeleteConferenceException(Guid id) : base($"Conference with ID: '{id}' cannot be deleted")
        {
            Id = id;
        }
    }
}