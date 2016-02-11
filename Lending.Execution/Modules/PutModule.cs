﻿using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.UnitOfWork;
using Lending.Execution.Auth;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Modules
{
    public abstract class PutModule<TMessage, TResult> : NancyModule where TMessage : AuthenticatedCommand where TResult : Result
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage, TResult> messageHandler;

        protected PutModule(IUnitOfWork unitOfWork, IMessageHandler<TMessage, TResult> messageHandler, string path)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            this.RequiresAuthentication();
            this.RequiresHttps();

            Put[path] = _ =>
            {
                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;

                TMessage message = this.Bind<TMessage>();
                message.UserId = user.Id;
                message.ProcessId = Guid.NewGuid();

                Result result = default(Result);
                unitOfWork.DoInTransaction(() =>
                {
                    result = messageHandler.Handle(message);
                });

                return new Response()
                {
                    StatusCode = (HttpStatusCode) result.Code,
                };

            };
        }
    }
}
