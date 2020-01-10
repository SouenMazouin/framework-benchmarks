package springbench.grpc;

import com.google.protobuf.Empty;
import com.google.protobuf.StringValue;
import io.grpc.ServerBuilder;
import springbench.repository.DbRepository;
import io.grpc.stub.StreamObserver;
import org.lognet.springboot.grpc.GRpcService;
import springbench.Queries;
import springbench.SpringServiceGrpc;
import springbench.World;
import springbench.WorldList;

import java.util.concurrent.ThreadLocalRandom;

@GRpcService
public class GrpcImpl extends SpringServiceGrpc.SpringServiceImplBase {

    private final DbRepository dbRepository;

    public GrpcImpl(DbRepository dbRepository) {
        this.dbRepository = dbRepository;
    }

    @Override
    public void threadTest(Empty request, StreamObserver<StringValue> responseObserver) {
        responseObserver.onNext(StringValue.newBuilder().setValue("Hello").build());
        String name = Thread.currentThread().getName();
        try {
            Thread.sleep(10000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        responseObserver.onCompleted();
    }

    @Override
    public void plainText(Empty request, StreamObserver<StringValue> responseObserver) {
        responseObserver.onNext(StringValue.newBuilder().setValue("Hello").build());
        responseObserver.onCompleted();
    }

    @Override
    public void singleQuery (Empty request, StreamObserver<World> responseObserver) {
        responseObserver.onNext(randomWorld());
        responseObserver.onCompleted();
    }

    @Override
    public void multiQueries(Queries request, StreamObserver<WorldList> responseObserver) {
        var worldList = WorldList.newBuilder();
        for (int i = 0; i < request.getNumber(); i++) {
            worldList.addWorld(randomWorld());
        }
        responseObserver.onNext(worldList.build());
        responseObserver.onCompleted();
    }

    @Override
    public void serverStream(Queries request, StreamObserver<World> responseObserver) {
        for (int i = 0; i < request.getNumber(); i++) {
            responseObserver.onNext(randomWorld());
        }
        responseObserver.onCompleted();
    }

    @Override
    public StreamObserver<Queries> bidiStream(StreamObserver<World> responseObserver) {
        return new StreamObserver<>() {
            @Override
            public void onNext(Queries queries) {
                for (int i = 0; i < queries.getNumber(); i++) {
                    World responseWorld = World.newBuilder()
                            .setId(randomWorld().getId())
                            .setRandomNumber(randomWorld().getRandomNumber())
                            .build();
                    responseObserver.onNext(responseWorld);
                }
            }

            public void onError(Throwable throwable) {
                responseObserver.onError(throwable);
            }

            @Override
            public void onCompleted() {
                responseObserver.onCompleted();
            }
        };
    }
    //TODO
    /*
    public StreamObserver<Queries>clientStream(StreamObserver<World> responseObserver){
        return new StreamObserver<World>() {
            int worldCount = 0;
            @Override
            public void onNext(World world) {
                worldCount++;
            }
            @Override
            public void onError(Throwable throwable) {
                responseObserver.onError(throwable);
            }
            @Override
            public void onCompleted() {
                responseObserver.onNext(Queries.newBuilder()
                        .setNumber(worldCount)
                        .build());
                responseObserver.onCompleted();
            }
        };
    }
    */

    private World randomWorld() {
        int random = randomWorldNumber();
        springbench.model.World world = dbRepository.getWorld(random);
        return World.newBuilder()
                .setId(world.id)
                .setRandomNumber(world.randomnumber)
                .build();
    }

    private static int randomWorldNumber() {
        return 1 + ThreadLocalRandom.current().nextInt(10000);
    }
}

