using System;

namespace TaskManager.Tests
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected TestServer Server { get; }
        protected IntegrationTestBase()
        {
            Server = new TestServer();
        }

        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}