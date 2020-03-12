# Credit Analysis Solution
This solution was developed using the 12-Factor APP document(https://12factor.net/) as base for the architecture. It will be composed as an Rest API for receive and handle the resquests related to Loans, an FIFO Queue to receive the resquests, a group of consumers that will receive from the queue and perform the approving process.

## Technology Stack

### Credit Analysis API
Will be developed in .NetCore that will be implemented inside a Docker Container to enable scalability and availability. This framework was choose because it contains a lot of builtin in funcionalties that expedited the development and testing of APIS, such as strongly typed language, model binding and validation, async processing, automatic documentation generation, dependecy injection, etc. The tests will be implemented using the xUnit framework

The API will be deployed on a container running on AWS Elastic Beanstalk for simplicity, but since it's stateless it can be deployed on any Kubernetes solution.

### FIFO Queue
Since all the tecnology implement will be AWS based SQS FIFO will be used to implement this part. It needs to maintain order in case of multiple requests of the same user be processed on the proper order and also to ensure that older users will be served first.

