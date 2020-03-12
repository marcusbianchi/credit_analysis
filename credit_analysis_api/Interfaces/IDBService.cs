using System.Threading.Tasks;
using credit_analysis_api;

public interface IDBService
{
    Task Insert(LoanRequest loan);
    Task<LoanRequest> Read(string uuid);
    Task<LoanRequest> Update(string uuid, LoanRequest loan);
}