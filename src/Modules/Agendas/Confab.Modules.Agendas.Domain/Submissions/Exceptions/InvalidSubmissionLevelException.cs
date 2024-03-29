﻿using System;
using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Agendas.Domain.Submissions.Exceptions
{
    public class InvalidSubmissionLevelException : ConfabException
    {
        public Guid SubmissionID { get; }

        public InvalidSubmissionLevelException(Guid submissionId) : base(
            $"Submission with ID: '{submissionId}' defines invalid level.")
            => SubmissionID = submissionId;
    }
}