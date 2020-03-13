using System.Data.Common;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;

namespace credit_analysis_consumer.Services
{
    public class ScorePolicy : IScorePolicy
    {
        private readonly IScoreService _scoreService;
        public ScorePolicy(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
        public async Task<Loan> ProcessScorePolicy(Loan loan)
        {
            var score = await _scoreService.GetScore(loan.cpf);
            if (score == null)
                loan.score_policy_result = null;

            if (score > 600)
                loan.score_policy_result = true;
            else
                loan.score_policy_result = false;
            return loan;
        }
    }
}