using System;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using EventMuncher;
using MbUnit.Framework;
using Rhino.Mocks;

namespace EventMucher.Tests
{
    [TestFixture]
    public class EventSourceRegistryTests
    {
        [Test]
        public void MakeSureTheRegistryLoadsConfigurationOnStartup()
        {
            // setup 
            IEventSourcesConfigurationProvider mockProvider
                = MockRepository.GenerateMock<IEventSourcesConfigurationProvider>();

            EventSourcesConfiguration configuration = new EventSourcesConfiguration();

            mockProvider
                .Expect(p => p.ProvideConfiguration())
                .Return(configuration);

            // execution
            EventSourceRegistry registry = new EventSourceRegistry(mockProvider);

            // checking
            Assert.AreEqual(5, registry.EventSourcesCount);

            mockProvider.VerifyAllExpectations();

            //Pes p = new Pes();
            //p.Laja().Laja().Scije().Laja().Laja().Poserje().Smrdi();
        }

        [Test]
        public void MakeSureTheRegistryLoadsConfigurationOnStartup2()
        {
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

            // execution
            EventSourceRegistry registry 
                = container.Resolve<EventSourceRegistry>();

            // checking
            Assert.AreEqual(5, registry.EventSourcesCount);
        }
    }

    //public class Pes
    //{
    //    public Pes Laja()
    //    {
    //        steviloLajezev++;
    //        return this;
    //    }

    //    public Pes Scije()
    //    {
    //        // whatever
    //        return this;
    //    }

    //    public Drekec Poserje()
    //    {
    //        return new Drekec();
    //    }

    //    private int steviloLajezev;
    //}

    //public class Drekec
    //{
    //    public void Smrdi()
    //    {
    //    }
    //}
}