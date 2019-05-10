# Framework Benchmarks

Benchmark of some Java and .NET frameworks


## Quick Start
To get started developing you'll need to install [docker](https://docs.docker.com/install/) for .NET framework and [maven](https://maven.apache.org/) for Java frameworks.

*Clone the repo:*
```
git clone https://github.com/SouenMazouin/framework-benchmarks.git
```
*Run:*

* For Spring-MVC & Spring Webflux benchmarks with Maven:

```
mvn clean package spring-boot:run
```

* For ASP.Net Core with Docker :
```
docker build -t netcorebench -f Dockerfile . 
docker run -p 6565:6565 -p 5000:80 -it --name netcore netcorebench
```

## Recommended Bench Tools

* **[WRK](https://github.com/wg/wrk)** is a modern HTTP benchmarking tool capable of generating significant load when run on a single multi-core CPU. It combines a multithreaded design with scalable event notification systems such as epoll and kqueue.

* **[GHZ](https://ghz.sh/)**
is a simple gRPC benchmarking and load testing tool inspired by
[hey](https://ghz.sh/) and [grpcurl](https://github.com/fullstorydev/grpcurl).
 
### Basic Usages

```bash
wrk -t8 -c512 -d15s http://localhost/plaintext
```

```bash
ghz -z 15s -cpus 8 -c 512 -proto {path-to-.proto} -insecure -call webfluxbench.WebfluxService.InMemory -d '{}' 0.0.0.0:6565
```





