﻿using System;
using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions
{
    public class EmptySubmissionTitleException : ConfabException
    {
        public Guid SubmissionID { get; }

        public EmptySubmissionTitleException(Guid submissionId) : base($"Submission with ID: '{submissionId}' defines empty title")
            => SubmissionID = submissionId;
    }
}