using credit_analysis_consumer;
using credit_analysis_consumer.Interfaces;
using credit_analysis_consumer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace credit_analysis_consumer_test
{
    public class ProcessRequestServiceTest
    {
        [Fact]
        public void ShouldReproveByAge()
        {
            var mockAgeDependency = new Mock<IAgePolicy>();

            mockAgeDependency.Setup(x => x.ProcessAgePolicy(It.IsAny<Loan>()))
                          .Returns(new Loan
                          {
                              age_policy_result = false
                          }
                          );
            var mockScoreDependency = new Mock<IScorePolicy>();


            var mockCommitmentDependency = new Mock<ICommitmentPolicy>();

            var mockQueue = new Mock<IQueueService>();

            var mockLoan = new Mock<ILoanService>();
            var mockLogger = new Mock<ILogger<ProcessRequestService>>();

            var processRequestService = new ProcessRequestService(mockAgeDependency.Object, mockScoreDependency.Object,
                                                                    mockCommitmentDependency.Object, mockQueue.Object,
                                                                    mockLoan.Object, mockLogger.Object);

            var (loan, requestPolicyResult) = processRequestService.ProcessLoan(new Loan()).Result;
            Assert.Equal(RequestPolicyResult.age, requestPolicyResult);
        }

        [Fact]
        public void ShouldReproveByScore()
        {
            var mockAgeDependency = new Mock<IAgePolicy>();

            mockAgeDependency.Setup(x => x.ProcessAgePolicy(It.IsAny<Loan>()))
                          .Returns(new Loan
                          {
                              age_policy_result = true
                          }
                          );
            var mockScoreDependency = new Mock<IScorePolicy>();

            mockScoreDependency.Setup(x => x.ProcessScorePolicy(It.IsAny<Loan>()))
                          .ReturnsAsync(new Loan
                          {
                              score_policy_result = false,
                              score = 100
                          }
                          );


            var mockCommitmentDependency = new Mock<ICommitmentPolicy>();

            var mockQueue = new Mock<IQueueService>();

            var mockLoan = new Mock<ILoanService>();
            var mockLogger = new Mock<ILogger<ProcessRequestService>>();

            var processRequestService = new ProcessRequestService(mockAgeDependency.Object, mockScoreDependency.Object,
                                                                    mockCommitmentDependency.Object, mockQueue.Object,
                                                                    mockLoan.Object, mockLogger.Object);

            var (loan, requestPolicyResult) = processRequestService.ProcessLoan(new Loan()).Result;
            Assert.Equal(RequestPolicyResult.score, requestPolicyResult);
            Assert.Equal(loan.score, 100);
        }

        [Fact]
        public void ShouldReproveByCommitment()
        {
            var mockAgeDependency = new Mock<IAgePolicy>();

            mockAgeDependency.Setup(x => x.ProcessAgePolicy(It.IsAny<Loan>()))
                          .Returns(new Loan
                          {
                              age_policy_result = true
                          }
                          );
            var mockScoreDependency = new Mock<IScorePolicy>();
            mockScoreDependency.Setup(x => x.ProcessScorePolicy(It.IsAny<Loan>()))
                          .ReturnsAsync(new Loan
                          {
                              score_policy_result = true,
                              score = 900
                          }
                          );

            var mockCommitmentDependency = new Mock<ICommitmentPolicy>();
            mockCommitmentDependency.Setup(x => x.ProcessCommitmentPolicy(It.IsAny<Loan>()))
                          .ReturnsAsync(new Loan
                          {
                              commitment_policy_result = false,
                              commitment_terms_result = 12,
                              commitment_terms_value = 500
                          }
                          );
            var mockQueue = new Mock<IQueueService>();

            var mockLoan = new Mock<ILoanService>();
            var mockLogger = new Mock<ILogger<ProcessRequestService>>();

            var processRequestService = new ProcessRequestService(mockAgeDependency.Object, mockScoreDependency.Object,
                                                                    mockCommitmentDependency.Object, mockQueue.Object,
                                                                    mockLoan.Object, mockLogger.Object);

            var (loan, requestPolicyResult) = processRequestService.ProcessLoan(new Loan()).Result;
            Assert.Equal(RequestPolicyResult.commitment, requestPolicyResult);
            Assert.Equal(12, loan.commitment_terms_result);
            Assert.Equal(500,loan.commitment_terms_value);

        }

        [Fact]
        public void ShouldApprove()
        {
            var mockAgeDependency = new Mock<IAgePolicy>();

            mockAgeDependency.Setup(x => x.ProcessAgePolicy(It.IsAny<Loan>()))
                          .Returns(new Loan
                          {
                              age_policy_result = true
                          }
                          );
            var mockScoreDependency = new Mock<IScorePolicy>();

            mockScoreDependency.Setup(x => x.ProcessScorePolicy(It.IsAny<Loan>()))
                          .ReturnsAsync(new Loan
                          {
                              score_policy_result = true,
                              score = 900
                          }
                          );

            var mockCommitmentDependency = new Mock<ICommitmentPolicy>();
            mockCommitmentDependency.Setup(x => x.ProcessCommitmentPolicy(It.IsAny<Loan>()))
                          .ReturnsAsync(new Loan
                          {
                              commitment_policy_result = true,
                              commitment_terms_result = 12,
                              commitment_terms_value = 500
                          }
                          );
            var mockQueue = new Mock<IQueueService>();

            var mockLoan = new Mock<ILoanService>();
            var mockLogger = new Mock<ILogger<ProcessRequestService>>();

            var processRequestService = new ProcessRequestService(mockAgeDependency.Object, mockScoreDependency.Object,
                                                                    mockCommitmentDependency.Object, mockQueue.Object,
                                                                    mockLoan.Object, mockLogger.Object);

            var (loan, requestPolicyResult) = processRequestService.ProcessLoan(new Loan()).Result;
            Assert.Null(requestPolicyResult);

        }
    }
}