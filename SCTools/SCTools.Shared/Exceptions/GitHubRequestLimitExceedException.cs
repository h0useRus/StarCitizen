using System;
using System.Net.Http;

namespace NSW.StarCitizen.Tools.Exceptions
{
    [Serializable]
    public class GitHubRequestLimitExceedException : HttpRequestException
    {
        public DateTimeOffset ResetLimitTime { get; }

        public GitHubRequestLimitExceedException(string message, DateTimeOffset resetLimitTime) : base(message)
        {
            ResetLimitTime = resetLimitTime;
        }
    }
}
