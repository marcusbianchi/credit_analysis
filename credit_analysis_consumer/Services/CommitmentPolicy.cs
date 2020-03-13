using System;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;

namespace credit_analysis_consumer.Services
{
    public class CommitmentPolicy : ICommitmentPolicy
    {
        private readonly ICommitmentService _commitmentService;
        public CommitmentPolicy(ICommitmentService commitmentService)
        {
            _commitmentService = commitmentService;
        }
        public async Task<Loan> ProcessCommitmentPolicy(Loan loan)
        {
            var commitment = await _commitmentService.GetCommitment(loan.cpf);
            var n = loan.terms;

            for (int i = n; i <= 12; i += 3)
            {
                var quota = CalculatQuota(loan.score, i, (double)loan.amount);
                if (quota == null)
                {
                    loan.commitment_policy_result = null;
                    loan.commitment_terms_result = null;
                    loan.commitment_terms_value = null;

                    return loan;
                }
                var available = (double)loan.income - ((double)commitment * (double)loan.income);
                if (quota < available)
                {
                    loan.commitment_policy_result = true;
                    loan.commitment_terms_result = i;
                    loan.commitment_terms_value = quota;
                    return loan;
                }
            }
            loan.commitment_policy_result = false;
            loan.commitment_terms_result = null;
            loan.commitment_terms_value = null;
            return loan;
        }

        private double? CalculatQuota(int score, int n, double amount)
        {
            var result = CalculteInterestRate(score, n);
            if (result == null)
                return null;
            var i = (double)result;
            var y = Math.Pow((1 + i), n);
            var quota = amount * ((y * i) / (y - 1));
            return quota;
        }

        private double? CalculteInterestRate(int score, int n)
        {
            if (600 >= score && score <= 699)
            {
                if (n == 6)
                    return 0.064;
                else if (n == 9)
                    return 0.066;
                else if (n == 12)
                    return 0.069;
                else
                    return null;
            }
            else if (700 >= score && score <= 799)
            {
                if (n == 6)
                    return 0.055;
                else if (n == 9)
                    return 0.058;
                else if (n == 12)
                    return 0.061;
                else
                    return null;
            }
            else if (800 >= score && score <= 899)
            {
                if (n == 6)
                    return 0.047;
                else if (n == 9)
                    return 0.050;
                else if (n == 12)
                    return 0.053;
                else
                    return null;
            }
            else
            {
                if (n == 6)
                    return 0.039;
                else if (n == 9)
                    return 0.042;
                else if (n == 12)
                    return 0.045;
                else
                    return null;
            }

        }
    }
}