# Framework Benchmarks

Benchmark of some Java and .NET frameworks

## Presentation


This series of benchmarks was originally designed to measure the difference between [Spring](https://docs.spring.io/spring/docs/current/spring-framework-reference/web.html) "traditional" use and its [reactive approach](https://docs.spring.io/spring/docs/current/spring-framework-reference/web-reactive.html#webflux).

Following this first series of results, we wanted to measure the results obtained between the HTTP/1 and [HTTP/2](https://en.wikipedia.org/wiki/HTTP/2) protocols on these same frameworks by using the [gRPC](https://grpc.io/) and [Protocol Buffer](https://developers.google.com/protocol-buffers/) technologies.

The C# [ASP.NET Core](https://docs.microsoft.com/fr-fr/aspnet/core/?view=aspnetcore-2.2) framework has also been added to this second approach to add some granularity to our benchmark series.

For Database Access we used [PgClient](https://libraries.io/github/reactiverse/reactive-pg-client) for Spring-Webflux, [JDBC](https://docs.oracle.com/cd/E11882_01/java.112/e16548/toc.htm) for Spring-MVC and [Dapper](https://dapper-tutorial.net/dapper) for .NET Core.

The entire project was built on the work of [Techempower](https://github.com/TechEmpower/FrameworkBenchmarks).

## Quick Start
To get started developing you'll need to install [docker](https://docs.docker.com/install/) for the .NET framework and for mount the databases, Java frameworks work with [maven](https://maven.apache.org/).

**Clone**
```
git clone https://github.com/SouenMazouin/framework-benchmarks.git
```
**Build & Run Data Bases**
* Mongo Db
```
docker build -t mongobench -f mongodb.dockerfile .
docker run -p 27017:27017 -it --name mongobench mongobench
```
* PostgreSQL
```
docker build -t postgresbench -f postgres.dockerfile .
docker run -p 5432:5432 -it --name postgresbench postgresbench
```

**Run servers**

* For Spring-MVC & Spring Webflux benchmarks with Maven

```
mvn clean package spring-boot:run
```

* For ASP.Net Core with Docker
```
docker build -t netcorebench -f Dockerfile . 
docker run -p 6565:6565 -p 5000:80 -it --name netcore netcorebench
```

## Recommended Bench Tools

* **[WRK](https://github.com/wg/wrk)** is a modern HTTP benchmarking tool capable of generating significant load when run on a single multi-core CPU. It combines a multithreaded design with scalable event notification systems such as epoll and kqueue.

* **[GHZ](https://ghz.sh/)**
is a simple gRPC benchmarking and load testing tool inspired by
[hey](https://github.com/rakyll/hey/) and [grpcurl](https://github.com/fullstorydev/grpcurl).
 
### Basic Usages

```bash
wrk -t8 -c512 -d15s http://localhost:8080/queries?queries=20
```

```bash
./ghz --insecure \
  -z 15s --cpus=8 -c 512 \
  --proto path/to/your/webfluxbench.proto \
  --call webfluxbench.WebfluxService.MultiQueries \
  -d '{"number":20}' \
  0.0.0.0:6565
```

## Resources

Methods / Techniques:
- [Reactive programming](https://www.reactivemanifesto.org/)
- [HTTP/2](https://http2.github.io/)

Technology:
- [Protocol Buffer](https://developers.google.com/protocol-buffers/)
- [gRPC](https://grpc.io/)

Frameworks:
- [Reactive-Spring](https://docs.spring.io/spring/docs/current/spring-framework-reference/web-reactive.html#webflux)
- [Spring](https://docs.spring.io/spring/docs/current/spring-framework-reference/web.html)
- [ASP.NET Core](https://docs.microsoft.com/fr-fr/aspnet/core/?view=aspnetcore-2.2) 

Database Acces:
- [PgClient](https://libraries.io/github/reactiverse/reactive-pg-client)
- [JDBC](https://docs.oracle.com/cd/E11882_01/java.112/e16548/toc.htm)
- [Dapper](https://dapper-tutorial.net/dapper)

References:
- [Techempower](http://www.techempower.com/)



