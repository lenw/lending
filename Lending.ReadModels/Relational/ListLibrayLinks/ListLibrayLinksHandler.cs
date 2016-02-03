﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Query;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibrayLinks
{
    public class ListLibrayLinksHandler : NHibernateQueryHandler<ListLibraryLinks, Result>,
        IAuthenticatedQueryHandler<ListLibraryLinks, Result>
    {
        public ListLibrayLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override Result Handle(ListLibraryLinks query)
        {
            LibraryLink libraryLinkAlias = null;
            OpenedLibrary acceptingLibraryAlias = null;
            OpenedLibrary requestingLibraryAlias = null;

            return new Result<LibraryLink[]>(Session.QueryOver<LibraryLink>(() => libraryLinkAlias)
                .JoinAlias(x => x.AcceptingLibrary, () => acceptingLibraryAlias)
                .JoinAlias(x => x.RequestingLibrary, () => requestingLibraryAlias)
                .Where(() => acceptingLibraryAlias.AdministratorId == query.UserId || requestingLibraryAlias.AdministratorId == query.UserId)
                .List()
                .ToArray());
        }
    }
}
