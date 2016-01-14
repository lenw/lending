﻿using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RequestLink;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForLibrary;

namespace Lending.Execution.Modules
{
    public class SearchForUserModule : GetModule<SearchForLibrary, Result>
    {
        public SearchForUserModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/libraries/{searchstring}";
    }

    public class RequestConnectionModule: PostModule<RequestLink, Result>
    {
        public RequestConnectionModule(IUnitOfWork unitOfWork, IMessageHandler<RequestLink, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/links/request/";
    }

    public class AcceptConnectionModule : PostModule<AcceptLink, Result>
    {
        public AcceptConnectionModule(IUnitOfWork unitOfWork, IMessageHandler<AcceptLink, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/links/accept/";
    }

    public class AddBookModule : PostModule<AddBookToLibrary, Result>
    {
        public AddBookModule(IUnitOfWork unitOfWork, IMessageHandler<AddBookToLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/books/add";
    }
}
