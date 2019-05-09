package webfluxbench.repository;

import webfluxbench.model.Fortune;
import webfluxbench.model.World;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public interface DbRepository {
    Mono<World> getWorld(int id);

    Mono<World> findAndUpdateWorld(int id, int randomNumber);

    Flux<Fortune> fortunes();
}