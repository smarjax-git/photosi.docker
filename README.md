

READ ReadMe.docx


The solution is made of:
- 1 REST API called photosi.api.
  You can interact with it through a SwaggerUI inteface.
  These REST API acts as a gateway to other 4 microservice:

  - photosi.users
  - photosi.orders
  - photosi.catalog
  - photosi.pickuppoints
  
Each microservice has:
  its own unit tests and integration tests in a separate project called test-{projec name}
  ans its own database hosted in the same instance of Sql Server Express.

The 5 api and the sql server instance can be execute in separate docker's container.
The solution includes the docker-compose file.

You just have to run: docker-compose up --build in the root folder.



