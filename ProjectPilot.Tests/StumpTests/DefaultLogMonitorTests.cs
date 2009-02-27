using System;
using System.IO;
using System.Threading;
using MbUnit.Framework;
using Stump.Services;

namespace ProjectPilot.Tests.StumpTests
{
    public class DefaultLogMonitorTests
    {
        [Test]
        public void MonitorFile()
        {
            const int Times = 5;

            using (DefaultLogMonitor monitor = new DefaultLogMonitor())
            {
                UpdateLogFile("test");

                monitor.StartMonitoring(
                    LogFileName,
                    LogFileCreated,
                    LogFileDeleted,
                    LogFileUpdated,
                    LogFileMonitorError);

                for (int i = 0; i < Times; i++)
                {
                    UpdateLogFile("test2" + i);
                    Thread.Sleep(100);
                }
            }

            Assert.AreEqual(0, errorCount);
            Assert.AreEqual(0, deletedCount);
            Assert.AreEqual(Times*2, updatedCount);
        }

        [Test]
        public void DeleteMonitoredFile()
        {
            using (DefaultLogMonitor monitor = new DefaultLogMonitor())
            {
                UpdateLogFile("test");

                monitor.StartMonitoring(
                    LogFileName,
                    LogFileCreated,
                    LogFileDeleted,
                    LogFileUpdated,
                    LogFileMonitorError);

                UpdateLogFile("test");

                Thread.Sleep(100);
                File.Delete(LogFileName);
                Thread.Sleep(100);
            }

            Assert.AreEqual(0, errorCount);
            Assert.AreEqual(2, updatedCount);
            Assert.AreEqual(1, deletedCount);
        }

        [Test]
        public void CreateMonitoredFile()
        {
            using (DefaultLogMonitor monitor = new DefaultLogMonitor())
            {
                monitor.StartMonitoring(
                    LogFileName,
                    LogFileCreated,
                    LogFileDeleted,
                    LogFileUpdated,
                    LogFileMonitorError);

                Thread.Sleep(100);
                UpdateLogFile("test");
                Thread.Sleep(100);
            }

            Assert.AreEqual(0, errorCount);
            Assert.AreEqual(1, createdCount);
            Assert.AreEqual(0, deletedCount);
            Assert.AreEqual(1, updatedCount);
        }

        /// <summary>Test case setup code.</summary>
        [SetUp]
        public void Setup()
        {
            if (File.Exists(LogFileName))
            {
                File.Delete(LogFileName);
                Thread.Sleep(200);
            }

            createdCount = 0;
            deletedCount = 0;
            errorCount = 0;
            updatedCount = 0;
        }

        private void LogFileCreated(string logFileName)
        {
            lock (this)
                createdCount++;
        }

        private void LogFileDeleted(string logFileName)
        {
            lock (this)
                deletedCount++;
        }

        private void LogFileMonitorError(string logFileName, Exception ex)
        {
            lock (this)
                errorCount++;
        }

        private void LogFileUpdated(string logFileName)
        {
            lock (this)
                updatedCount++;
        }

        private void UpdateLogFile(string contents)
        {
            File.WriteAllText(DefaultLogMonitorTests.LogFileName, contents);
        }

        private int createdCount;
        private int deletedCount;
        private int errorCount;
        private const string LogFileName = "test.log";
        private int updatedCount;
    }
}