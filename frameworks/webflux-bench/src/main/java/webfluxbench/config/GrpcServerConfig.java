package webfluxbench.config;

import io.grpc.ServerBuilder;
import org.lognet.springboot.grpc.GRpcServerBuilderConfigurer;
import org.springframework.stereotype.Component;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

@Component
public class GrpcServerConfig extends GRpcServerBuilderConfigurer {

    private static final ExecutorService EXECUTOR_SERVICE =
            Executors.newFixedThreadPool(2 * Runtime.getRuntime().availableProcessors());

    @Override
    public void configure(ServerBuilder<?> serverBuilder) {
        serverBuilder.executor(EXECUTOR_SERVICE);
    }
}
