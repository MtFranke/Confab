using System;
using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions
{
    public class EmptySubmissionDescriptionException : ConfabException
    {
        public Guid SubmissionID { get; }

        public EmptySubmissionDescriptionException(Guid submissionId) : base(
            $"Submission with ID: '{submissionId}' defines empty description")
            => SubmissionID = submissionId;
    }
}