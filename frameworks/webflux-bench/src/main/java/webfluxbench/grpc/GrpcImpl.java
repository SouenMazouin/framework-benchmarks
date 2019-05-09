package webfluxbench.grpc;

import webfluxbench.repository.DbRepository;
import com.google.protobuf.Empty;
import com.google.protobuf.StringValue;
import org.lognet.springboot.grpc.GRpcService;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;
import webfluxbench.Queries;
import webfluxbench.ReactorWebfluxServiceGrpc;
import webfluxbench.World;
import webfluxbench.WorldList;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@GRpcService
public class GrpcImpl extends ReactorWebfluxServiceGrpc.WebfluxServiceImplBase{

    private final DbRepository dbRepository;

    public GrpcImpl(DbRepository dbRepository) {
        this.dbRepository = dbRepository;
    }

    @Override
    public Mono<StringValue> inMemory(Mono<Empty> request) {
        return Mono.just(StringValue.newBuilder().setValue("Hello, World!").build());
    }

    @Override
    public Mono<World> singleQuery(Mono<Empty> request) {
        return randomWorld();
    }

    @Override
    public Mono<WorldList> multiQueries(Mono<Queries> request){
        return getWorlds(request.flux())
                .collect(WorldList::newBuilder, WorldList.Builder::addWorld)
                .map(WorldList.Builder::build);
    }

    @Override
    public Flux<World> serverStream(Mono<Queries> request) {
         return getWorlds(request.flux());
    }

    @Override
    public Flux<World> bidiStream(Flux<Queries> request) {
        return getWorlds(request);
    }

    @Override
    public Mono<World> clientStream(Flux<Queries> request){
        return getWorlds(request).last();
    }

    private Flux<World> getWorlds(Flux<Queries> request) {
        return request
                .map(Queries::getNumber)
                .map(this::getWorldsFromDb)
                .flatMap(Flux::merge);
    }

    private List<Mono<World>> getWorldsFromDb(Integer number) {
        return Stream.generate(this::randomWorld)
                .limit(number)
                .collect(Collectors.toList());
    }

    private Mono<World> randomWorld() {
        return dbRepository.getWorld(randomWorldNumber())
                .map(GrpcImpl::toProto);
    }

    private static int randomWorldNumber() {
        return 1 + ThreadLocalRandom.current().nextInt(10000);
    }

    private static World toProto(webfluxbench.model.World world) {
        return World.newBuilder()
                .setId(world.id)
                .setRandomNumber(world.randomnumber)
                .build();
    }

}
