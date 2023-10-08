namespace DevOpsKata.Common;

public interface IDevOpsKataCommand
{
    Task ExecuteAsync(CancellationToken token);
}
