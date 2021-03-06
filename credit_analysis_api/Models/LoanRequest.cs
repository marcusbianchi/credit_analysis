public class LoanRequest
{
    public string id { get; set; }
    public RequestStatus? status { get; set; }
    public RequestResult? result { get; set; }
    public RequestPolicyResult? refused_policy { get; set; }
    public decimal? amount { get; set; }
    public int? terms { get; set; }
}
public enum RequestPolicyResult
{
    age, score, commitment
}
public enum RequestResult
{
    approved, refused
}
public enum RequestStatus
{
    processing, completed
}