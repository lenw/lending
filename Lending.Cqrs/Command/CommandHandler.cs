﻿using System;
using Lending.Cqrs.Query;

namespace Lending.Cqrs.Command
{
    public abstract class CommandHandler<TMessage, TResult, TAggregate> : MessageHandler<TMessage, TResult>, 
        ICommandHandler<TMessage, TResult, TAggregate> where TMessage : Command where TResult : Result where TAggregate : Aggregate
    {
        private readonly Func<IRepository> getRepository;
        private readonly Func<IEventRepository> getEventRepository;

        protected CommandHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
        {
            this.getRepository = repositoryFunc;
            this.getEventRepository = eventRepositoryFunc;
        }

        protected IRepository Session => getRepository();

        protected IEventRepository EventRepository => getEventRepository();

        protected virtual Result Success()
        {
            return new Result();
        }

        protected virtual Result Fail(string reason)
        {
            return new Result(reason);
        }

    }
}
