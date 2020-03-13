using System;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;

namespace credit_analysis_consumer.Services
{
    public class AgePolicy : IAgePolicy
    {
        public Loan ProcessAgePolicy(Loan loan)
        {
            var age = ((DateTime.Now - loan.birthdate).TotalDays) / 365;
            if (age >= 18)
                loan.age_policy_result = true;
            else
                loan.age_policy_result = false;
            return loan;
        }
    }
}