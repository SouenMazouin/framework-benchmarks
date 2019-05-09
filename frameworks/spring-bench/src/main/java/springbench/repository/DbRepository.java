package springbench.repository;

import springbench.model.Fortune;
import springbench.model.World;

import java.util.List;

public interface DbRepository {
    World getWorld(int id);

    World updateWorld(World world, int randomNumber);

    List<Fortune> fortunes();
}
