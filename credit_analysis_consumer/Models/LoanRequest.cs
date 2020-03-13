public class LoanRequest
{
    public string id { get; set; }
    public RequestStatus? status { get; set; }
    public RequestResult? result { get; set; }
    public RequestPolicyResult? refused_policy { get; set; }
    public decimal? amount { get; set; }
    public int? term { get; set; }
}
public enum RequestPolicyResult
{
    approved, refused
}
public enum RequestResult
{
    approved, refused
}
public enum RequestStatus
{
    processing, completed
}