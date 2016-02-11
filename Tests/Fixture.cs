﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Execution;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;
using StructureMap;

namespace Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            this.SetupContainer(new TestRegistry());
            this.SetUpOwinServer<Startup>();

        }

        [TearDown]
        public virtual void TearDown()
        {
            this.TearDownOwinServer();
        }

    }
}
