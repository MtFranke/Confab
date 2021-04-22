using System;
using Confab.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;

namespace Confab.Shared.Infrastructure.Contexts
{
    internal class Context : IContext
    {
        public string RequestId { get; } = $"{Guid.NewGuid():N}";
        public string TraceId { get; }
        public IIdentityContext Identity { get; }

        public Context(HttpContext context): this(context.TraceIdentifier, new IdentityContext(context.User) )
        {
            
        }

        internal Context()
        {
        }

        internal Context(string traceId, IIdentityContext context)
        {
            TraceId = traceId;
            Identity = context;
        }
        public static IContext Empty => new Context();

    }
}