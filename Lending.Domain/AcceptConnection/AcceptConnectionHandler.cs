using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionHandler : AuthenticatedCommandHandler<AcceptConnection, Result>
    {
        public AcceptConnectionHandler(Func<ISession> getSession, Func<IRepository> getRepository) 
            : base(getSession, getRepository)
        {
        }

        public override Result HandleCommand(AcceptConnection command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));

            Result result = user.AcceptConnection(command.ProcessId, command.RequestingUserId);

            if (!result.Success) return result;

            Repository.Save(user);

            return result;
        }
    }
}