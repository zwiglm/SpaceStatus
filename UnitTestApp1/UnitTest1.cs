using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SpaceStatusPoller;

namespace UnitTestApp1
{
    [TestClass]
    public class UnitTest1
    {
        private SpaceStatusPoll _poll;

        [TestInitialize]
        public void Initialize()
        {
            _poll = new SpaceStatusPoll();
        }

        [TestMethod]
        public void TestMethod1()
        {
            _poll.Run(null);
        }
    }
}
