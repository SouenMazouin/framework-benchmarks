package springbench.repository;

import springbench.model.Fortune;
import springbench.model.World;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Repository;

import java.util.Collections;
import java.util.List;

@Repository
@Profile("memory")
public class InMemoryRepository implements DbRepository {
    @Override
    public World getWorld(int id) {
        return new World(id, id);
    }

    @Override
    public World updateWorld(World world, int randomNumber) {
        world.randomnumber = randomNumber;
        return world;
    }

    @Override
    public List<Fortune> fortunes() {
        return Collections.singletonList(new Fortune(42, "Hello wworld"));
    }
}
