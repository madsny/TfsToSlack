namespace TfsPoller
{
    public class TestStatistics
    {
        public int PassedTests { get; private set; }
        public int FailedTests { get; private set; }
        public int TotalTests { get; private set; }

        public TestStatistics(int passedTests = 0, int failedTests = 0, int totalTests = 0)
        {
            PassedTests = passedTests;
            FailedTests = failedTests;
            TotalTests = totalTests;
        }

        public override string ToString()
        {
            return string.Format("Passed: {0}, Failed: {1}, Total: {2}", PassedTests, FailedTests, TotalTests);
        }
    }
}