# Credit Analysis Solution
This solution was developed using the 12-Factor APP document(https://12factor.net/) as base for the architecture. It will be composed as an Rest API for receive and handle the requests related to Loans, an FIFO Queue to receive the requests, a group of consumers that will receive from the queue and perform the approving process and a database to store the results.


## Table of Contents
- [Credit Analysis Solution](#credit-analysis-solution)
  * [Technology Stack](#technology-stack)
    + [Credit Analysis API and Consumers](#credit-analysis-api-and-consumers)
    + [FIFO Queue](#fifo-queue)
    + [Database](#database)
  * [Deployment Procedure with Docker](#deployment-procedure-with-docker)
    + [Credit Analysis API](#credit-analysis-api)
    + [Credit Analysis Consumer](#credit-analysis-consumer)
  * [Deployment Procedure with Docker Compose](#deployment-procedure-with-docker-compose)
  * [Unit Testing](#unit-testing)
  * [Future Improvements](#future-improvements)

## Technology Stack

### Credit Analysis API and Consumers
Will be developed in .NetCore that will be implemented inside a Docker Container to enable scalability and availability. This framework was choose because it contains a lot of builtin in functionalities that expedited the development and testing of APIS, such as strongly typed language, model binding and validation, async processing, automatic documentation generation, dependency injection, etc. The tests will be implemented using the xUnit framework

The API and Consumers will can deployed on a container running on AWS Elastic Beanstalk for simplicity, but since it's stateless it can be deployed on any Kubernetes solution.

### FIFO Queue
Since all the technology implement will be AWS based SQS FIFO will be used to implement this part. It needs to maintain order in case of multiple requests of the same user be processed on the proper order and also to ensure that older users will be served first.

### Database
Since all the technology implement will be AWS based DynamoDB will be used to implement this part. Since all the data is JSON based and the queries wil be mainly on non-nested JSONs and using ID the solution will be able to handle the requests.

## Deployment Procedure with Docker

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
	--network host \ Guid.NewGuid().ToString()
	--name credit_api marcusbianchi/credit_analysis_api
```

**The API Swagger UI will be available in http://localhost:80, it will also have all the documentation to use the API.**

Sample request:
```shell
curl -X POST "http://localhost:80/loan" -H "accept: */*" -H "Content-Type: application/json" -d "{\"name\":\"Jonny Rocket\",\"cpf\":\"20699988822\",\"birthdate\":\"2000-03-15\",\"amount\":1000,\"terms\":6,\"income\":1000}"
```

__OBS: This test version is using host network and shouldn't be used in production.__

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
**The consumer will start in a container and start to process the QUEUE.**

__OBS: This test version is using host network and shouldn't be used in production.__

## Deployment Procedure with Docker Compose

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
- DOCKER COMPOSE

Follow the following steps:
1. Update the file at "./deploy/docker-compose/docker-compose.yml" with the proper variables
2. Run the Docker Compose command
```shell
cd ./deploy/docker-compose/docker-compose.yml
docker-compose up -d
```

**The API Swagger UI will be available in http://localhost:8080, it will also have all the documentation to use the API.**

Sample request:
```shell
curl -X POST "http://localhost:8080/loan" -H "accept: */*" -H "Content-Type: application/json" -d "{\"name\":\"Jonny Rocket\",\"cpf\":\"20699988822\",\"birthdate\":\"2000-03-15\",\"amount\":1000,\"terms\":6,\"income\":1000}"
```

**The consumer will start in a container and start to process the QUEUE.**


## Testing	

It was used a TDD approach to develop this project but to run the test first you need to configure some environment variables that will be used because there are some integration tests as well:
```shell
export SCORE_URL=xxx
export SCORE_KEY=xxx
export COMMITMENT_URL=xxx
export COMMITMENT_KEY=xxx
export QUEUE_URL=xxx
export SQS_SERVICE_URL=https://sqs.us-east-1.amazonaws.com/
```

After that just run the .netcore test command for each project:
- Consumer:
```shell
dotnet test ./credit_analysis_consumer_test/
```
- API:
```shell
dotnet test ./credit_analysis_api_test/
```

## Future Improvements
- Add Terraform scripts to create and run the infrastructure