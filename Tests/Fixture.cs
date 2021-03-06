﻿using System;
using Joshilewis.Infrastructure;
using Lending.Execution.DI;
using LightBDD;
using LightBDD.Coordination;
using LightBDD.Notification;
using NUnit.Framework;
using Tests;
using static Joshilewis.Testing.Helpers.DIExtensions;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.MigrationExtensions;

namespace Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        protected BDDRunner Runner { get; private set; }

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            Runner = new BDDRunner(this.GetType(), CreateProgressNotifier());
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            FeatureCoordinator.Instance.AddFeature(Runner.Result);
        }

        protected virtual IProgressNotifier CreateProgressNotifier()
        {
            return new ConsoleProgressNotifier();
        }

        [SetUp]
        public virtual void SetUp()
        {
            SetUpDependencyProvision<LendingContainer>(new TestRegistry());
            SetUpOwinServer<Startup>();
            SetUpEventStore();
            BuildSchema();
        }

        [TearDown]
        public virtual void TearDown()
        {
            DropSchema();
            TearDownEventStore();
            TearDownOwinServer();
        }

        protected virtual void Given(Action action)
        {
            action();
        }

        protected virtual void When(Action action)
        {
            action();
        }

        protected virtual void Then(Action action)
        {
            action();
        }

    }


}
