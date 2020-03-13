# Credit Analysis Solution
This solution was developed using the 12-Factor APP document(https://12factor.net/) as base for the architecture. It will be composed as an Rest API for receive and handle the resquests related to Loans, an FIFO Queue to receive the resquests, a group of consumers that will receive from the queue and perform the approving process.

## Technology Stack

### Credit Analysis API and Consumers
Will be developed in .NetCore that will be implemented inside a Docker Container to enable scalability and availability. This framework was choose because it contains a lot of builtin in funcionalties that expedited the development and testing of APIS, such as strongly typed language, model binding and validation, async processing, automatic documentation generation, dependecy injection, etc. The tests will be implemented using the xUnit framework

The API and Consumers will can deployed on a container running on AWS Elastic Beanstalk for simplicity, but since it's stateless it can be deployed on any Kubernetes solution.

### FIFO Queue
Since all the tecnology implement will be AWS based SQS FIFO will be used to implement this part. It needs to maintain order in case of multiple requests of the same user be processed on the proper order and also to ensure that older users will be served first.

## Deployment Procedure

You will need an AWS account with:
- DynamoDB Table Configured
- SQS FIFO Configured
- AWS ACCESS KEY ID
- AWS SECRET KEY
- COMMITMENT URL
- COMMITMENT KEY
- SCORE URL
- SCORE KEY
- DOCKER


### Credit Analysis API
This API is contained in a Docker container published on marcusbianchi/credit_analisys_api. To run this container locally use the following command:

```shell
docker run -d \
	--env AWS_ACCESS_KEY_ID=xxx \
	--env AWS_SECRET_KEY=xxx \
	--env QUEUE_URL=https://sqs.us-east-1.amazonaws.com/066105611759/xxx.fifo \
	--env SQS_SERVICE_URL=https://sqs.us-east-1.amazonaws.com/ \
	--env DYNAMO_SERVICE_URL=https://dynamodb.us-east-1.amazonaws.com/ \
	--env AWS_DEFAULT_REGION=us-east-1 \
	--env DYNAMO_TABLE=xxx \
	--network host \
	--name credit_api marcusbianchi/credit_analysis_api
```

The API Swagger UI will be available in http://localhost:80, it will also have all the documentation to use the API.

OBS: This test version is using host network and shouldn't be used in production.

### Credit Analysis Consumer
This Worker is contained in a Docker container published on marcusbianchi/credit_analysis_consumer. To run this container locally use the following command:

```shell
docker run \
	--env AWS_ACCESS_KEY_ID=xxx \
	--env AWS_SECRET_KEY=xxx \
	--env QUEUE_URL=https://sqs.us-east-1.amazonaws.com/066105611759/xx.fifo \
	--env SQS_SERVICE_URL=https://sqs.us-east-1.amazonaws.com/ \
	--env SCORE_URL=xxx \
    --env SCORE_KEY=xxx \
	--env AWS_DEFAULT_REGION=us-east-1 \
    --env COMMITMENT_URL=xx \
    --env COMMITMENT_KEY=xxx \
    --env LOAN_URL=http://localhost:80/loan \
    --network host \
	--name credit_consumer marcusbianchi/credit_analysis_consumer
```
The consumer will start in a container as start to read the QUEUE.

OBS: This test version is using host network and shouldn't be used in production.

