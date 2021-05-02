using System;
using System.Threading.Tasks;
using Confab.Modules.Agendas.Application.Submissions.Commands;
using Confab.Modules.Conferences.Api.Controllers;
using Confab.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Agendas.Api.Controllers
{
    internal class SubmissionController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public SubmissionController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateSubmission submission)
        {
            await _commandDispatcher.SendAsync(submission);
            return new CreatedResult("Get", new { });
        }

        [HttpPut("{id:guid}/approve")]
        public async Task<ActionResult> ApproveAsync(Guid id)
        {
            await _commandDispatcher.SendAsync(new ApproveSubmission(id));
            return NoContent();
        }

        [HttpPut("{id:guid}/reject")]
        public async Task<ActionResult> RejectAsync(Guid id)
        {
            await _commandDispatcher.SendAsync(new RejectSubmission(id));
            return NoContent();
        }
    }
}