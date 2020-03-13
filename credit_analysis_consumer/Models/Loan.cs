using System;
using System.ComponentModel.DataAnnotations;

namespace credit_analysis_consumer
{
    public class Loan
    {
        public string id { get; set; }

        public string name { get; set; }

        public string cpf { get; set; }

        public DateTime birthdate { get; set; }

        [Range(1000, 4000)]
        public decimal amount { get; set; }

        [RegularExpression(@"^(6|9|12){1}$",
         ErrorMessage = "Number of therms must be 8, 9 or 12.")]
        public int terms { get; set; }

        public decimal income { get; set; }

        public bool? score_policy_result { get; set; }
    }
}
