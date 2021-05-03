using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Agendas.Application.Services;
using Confab.Modules.Agendas.Application.Submissions.Exceptions;
using Confab.Modules.Agendas.Domain.Submissions.Repositories;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Kernel;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Agendas.Application.Submissions.Commands.Handlers
{
    public class ApproveSubmissionHandler : ICommandHandler<ApproveSubmission>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;

        public ApproveSubmissionHandler(ISubmissionRepository submissionRepository, IDomainEventDispatcher domainEventDispatcher, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _submissionRepository = submissionRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }

        public async Task HandleAsync(ApproveSubmission command)
        {
            var submission = await _submissionRepository.GetAsync(command.Id);
            if (submission is null)
            {
                throw new SubmissionNotFoundException(command.Id);
            }

            submission.Approve();
            await _submissionRepository.UpdateAsync(submission);
            
            var events = _eventMapper.MapAll(submission.Events);
            await _messageBroker.PublishAsync(events.ToArray());

            
            
            await _domainEventDispatcher.DispatchAsync(submission.Events.ToArray());
        }
    }
}