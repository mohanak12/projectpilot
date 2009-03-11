using System.IO;
using System.Threading;
using MbUnit.Framework;
using Rhino.Mocks;
using Stump.Services;

namespace ProjectPilot.Tests.StumpTests
{
    [TestFixture]
    public class DefaultLogUpdaterQueueTests
    {
        [Test, Pending]
        public void ReadLog()
        {
            callbacksCount = 0;
            ILogReader logReader = MockRepository.GenerateMock<ILogReader>();
            logReader.Expect(lr => lr.ReadLog("test.log"))
                .Do((Delegates.Function<string, string>)delegate 
                { 
                    Thread.Sleep(1000);
                    return "contents"; 
                })
                .Repeat.Once();

            DefaultLogUpdaterQueue updaterQueue = new DefaultLogUpdaterQueue(logReader);
            updaterQueue.FetchLogContents("test.log", Callback);
            updaterQueue.FetchLogContents("test.log", Callback);

            //Assert.AreEqual(0, updaterQueue.RequestsInQueue);
            Assert.AreEqual(1, callbacksCount);
            logReader.VerifyAllExpectations();
        }

        private void Callback(LogUpdateRequest request)
        {
            lock (this)
                callbacksCount++;
        }

        private int callbacksCount;
    }
}