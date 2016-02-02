﻿using System;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/7
    /// As a Library I want to Accept a Requested Link from another Library So that we can see each other's Books.
    /// </summary>
    [TestFixture]
    public class AcceptLinkTests : FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN Library1 AND Library2 are Open AND they are not Linked AND Library1 has requested to Link to Library2
        ///WHEN Library2 accepts the Link Request from Library1
        ///THEN Library1 and Library2 are connected
        /// </summary>
        [Test]
        public void AcceptLinkForUnconnectedLibrarysWithAPendingRequestShouldSucceed()
        {
            Given(Library1Opens, Library2Opens);
            Given(Library1RequestsLinkToLibrary2, "links/request");
            WhenCommand(Library2AcceptsLinkFromLibrary1).IsPUTedTo("links/accept");
            Then(Http200Ok);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, DefaultTestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, DefaultTestData.LinkAccepted);
        }

        /// <summary>
        ///GIVEN Library1 AND Library2 are Open AND they are not Linked AND there are no Link Requests between them
        ///WHEN Library2 accepts the Link Request from Library1
        ///THEN No Link is made and Library2 is informed that the acceptance failed because there is no Link Request between them
        /// </summary>
        [Test]
        public void AcceptLinkWithNoPendingRequestShouldFail()
        {
            Given(Library1Opens, Library2Opens);
            WhenCommand(Library2AcceptsLinkFromLibrary1).IsPUTedTo("links/accept");
            Then(Http400BecauseNoPendingLinkRequest);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened);
        }

        /// <summary>
        ///GIVEN Library1 AND Library2 are open AND they are Linked
        ///WHEN Library2 accepts the Link Request from Library1
        ///THEN No Link is made and Library2 is informed that the acceptance failed because they are already Linked
        /// </summary>
        [Test]
        public void AcceptLinkForLinkedLibrariesShouldFail()
        {
            Given(Library1Opens, Library2Opens);
            Given(Library1RequestsLinkToLibrary2, "links/request");
            Given(Library2AcceptsLinkFromLibrary1, "links/accept");
            WhenCommand(Library2AcceptsLinkFromLibrary1).IsPUTedTo("links/accept");
            Then(Http400BecauseLibrariesAlreadyLinked);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2, DefaultTestData.LinkCompleted);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received, DefaultTestData.LinkAccepted);
        }

        [Test]
        public void AcceptLinkByUnauthorizedUserShouldFail()
        {
            Given(Library1Opens, Library2Opens);
            Given(Library1RequestsLinkToLibrary2, "links/request");
            WhenCommand(UnauthorizedAcceptLink).IsPUTedTo("links/accept");
            Then(Http403BecauseUnauthorized(UnauthorizedAcceptLink.UserId,
                UnauthorizedAcceptLink.AggregateId, typeof(Library)));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, LinkRequestedFrom1To2);
            AndEventsSavedForAggregate<Library>(Library2Id, Library2Opened, LinkRequestFrom1To2Received);
        }

    }
}
