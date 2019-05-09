package springbench.repository;

import springbench.model.Fortune;
import springbench.model.World;
import org.springframework.context.annotation.Profile;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.stereotype.Component;

import java.util.List;

@Component
@Profile("mongo")
public class MongoDbRepository implements DbRepository {
    private final MongoTemplate mongoTemplate;

    public MongoDbRepository(MongoTemplate mongoTemplate) {
        this.mongoTemplate = mongoTemplate;
    }

    @Override
    public World getWorld(int id) {
        return mongoTemplate.findById(id, World.class);
    }

    @Override
    public World updateWorld(World world, int randomNumber) {
        world.randomnumber = randomNumber;
        return mongoTemplate.save(world);
    }

    @Override
    public List<Fortune> fortunes() {
        return mongoTemplate.findAll(Fortune.class);
    }
}
