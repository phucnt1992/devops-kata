using DevOpsKata.Common;

namespace DevOpsKata.SslExpiryDetector.Commands;
public class SslDetectCommandOptions : IDevOpsKataOptions
{
    public string Domain { get; set; } = string.Empty;
    public TimeSpan Threshold { get; set; } = TimeSpan.FromDays(30);
    public bool Verbose { get; set; }
}
