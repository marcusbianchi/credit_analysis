using System;
using System.ComponentModel.DataAnnotations;

namespace credit_analysis_api
{
    public class Loan
    {
        public string id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$",
        ErrorMessage = "The strng must be numeric only with 11 characteres.")]
        [StringLength(11, ErrorMessage = "The strng must be numeric only with 11 characteres.", MinimumLength = 11)]
        public string cpf { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime birthdate { get; set; }
        [Required]
        [Range(1000.00, 4000.00)]
        public decimal amount { get; set; }
        [Required]
        [RegularExpression(@"^(6|9|12){1}$",
             ErrorMessage = "Number of therms must be 8, 9 or 12.")]
        public int terms { get; set; }
        [Required]
        public decimal income { get; set; }
    }
}
