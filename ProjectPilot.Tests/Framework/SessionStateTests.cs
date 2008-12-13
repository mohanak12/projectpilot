using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.Framework;

namespace ProjectPilot.Tests.Framework
{
    /// <summary>
    /// Tests the <see cref="FileSessionStorage"/> and <see cref="SessionState"/> classes.
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(FileSessionStorage))]
    [TestsOn(typeof(SessionState))]
    public class SessionStateTests
    {
        /// <summary>
        /// Tests creating of a new <see cref="ISessionState"/> object.
        /// </summary>
        [Test]
        public void FetchEmptySession()
        {
            using (ISessionState sessionState = sessionStorage.LoadSession("testId"))
            {
                Assert.IsNotNull(sessionState);
            }
        }

        /// <summary>
        /// Saves the session state for the first time.
        /// </summary>
        [Test]
        public void SaveSessionFirstTime()
        {
            // set the value
            using (ISessionState sessionState = sessionStorage.LoadSession("testId"))
            {
                Assert.IsNotNull(sessionState);

                sessionState.SetValue("testvalue", "test");
            }

            // now read it from the session storage
            using (ISessionState sessionState = sessionStorage.LoadSession("testId"))
            {
                Assert.AreEqual("test", sessionState.GetValue<string>("testvalue"));
            }
        }

        /// <summary>
        /// Test case setup code.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            fileManager = new DefaultFileManager("Storage", null);
            sessionStorage = new FileSessionStorage(fileManager);
            sessionStorage.ClearSession(SessionHolderId);
        }

        private IFileManager fileManager;
        private const string SessionHolderId = "testId";
        private FileSessionStorage sessionStorage;
    }
}
