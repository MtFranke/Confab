using System;
using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions
{
    public class InvalidSubmissionStatusException : ConfabException
    {
        public Guid SubmissionID { get; }

        public InvalidSubmissionStatusException(
            Guid submissionId, string desiredStatus, string currentStatus) : base(
            $"Cannot change status of submission with ID: '{submissionId}' from {currentStatus} to {desiredStatus}")
            => SubmissionID = submissionId;
    }
}