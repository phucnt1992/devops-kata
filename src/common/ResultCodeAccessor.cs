namespace DevOpsKata.Common;
public class ResultCodeAccessor
{
    public int ResultCode { get; set; }

    public void SetResultCode(ResultCodes resultCode)
    {
        ResultCode = int.Parse(resultCode.ToString("D"));
    }
}
