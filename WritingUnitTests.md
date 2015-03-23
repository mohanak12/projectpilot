Example test mathod:
```
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
```