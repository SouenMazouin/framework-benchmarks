# Spring Webflux Benchmarking Test

This is the Spring Webflux portion of a [benchmarking test suite](../) comparing a variety of web development platforms.

Netty is used for the async web server, with nearly everything configured with default settings. The only thing changed is Hikari can use up to (2 * cores count) connections (the default is 10). See [About-Pool-Sizing](https://github.com/brettwooldridge/HikariCP/wiki/About-Pool-Sizing)

A fixed thread pool of size equals to the number of database connections is used to run all the blocking code (jdbc database accesses) to not block netty's event loop.

For postgresql access, there are four implementations.
* [JdbcDbRepository](src/main/java/webfluxbench/repository/JdbcDbRepository.java) is using JdbcTemplate.
* [PgClientDbRepository](src/main/java/webfluxbench/repository/PgClientDbRepository.java) is using reactive-pg-client
For mongoDB access, spring-data-mongodb with reactive support is used. See [MongoDbRepository](src/main/java/webfluxbench/repository/MongoDbRepository.java)

### Plaintext Test

* [Plaintext test source](src/main/java/webfluxbench/web/WebfluxRouter.java)

### JSON Serialization Test

* [JSON test source](src/main/java/webfluxbench/web/WebfluxRouter.java)

### Database Query Test

* [Query test source](src/main/java/webfluxbench/web/WebfluxRouter.java)

### Database Queries Test

* [Queries test source](src/main/java/webfluxbench/web/WebfluxRouter.java)

### Database Update Test

* [Update test source](src/main/java/webfluxbench/web/WebfluxRouter.java)

### Template rendering Test

* [Template rendering test source](src/main/java/webfluxbench/web/WebfluxRouter.java)

## Test URLs

### Plaintext Test

    http://localhost:8080/plaintext

### Database Query Test

    http://localhost:8080/db

### Database Queries Test

    http://localhost:8080/queries?queries=5

### Template rendering Test

    http://localhost:8080/fortunes
