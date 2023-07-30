using System.Collections.ObjectModel;

namespace Puzzler;

public class Context
{
	public ObservableCollection<Level> Levels { get; } = new ObservableCollection<Level>();

	public Context()
	{
		Levels.Add(new Level { Name = "First", ImageName = "first.jpg", Difficulty = LevelDifficulty.Easy });
        Levels.Add(new Level { Name = "Cheetah", ImageName = "cheetah.jpg", Difficulty = LevelDifficulty.Easy });
        Levels.Add(new Level { Name = "Koala", ImageName = "koala.jpg", Difficulty = LevelDifficulty.Easy });
        Levels.Add(new Level { Name = "Giraffe", ImageName = "giraffe.jpg", Difficulty = LevelDifficulty.Easy });

        Levels.Add(new Level { Name = "First", ImageName = "first.jpg", Difficulty = LevelDifficulty.Medium });
        Levels.Add(new Level { Name = "Cheetah", ImageName = "cheetah.jpg", Difficulty = LevelDifficulty.Medium });
        Levels.Add(new Level { Name = "Koala", ImageName = "koala.jpg", Difficulty = LevelDifficulty.Medium });
        Levels.Add(new Level { Name = "Giraffe", ImageName = "giraffe.jpg", Difficulty = LevelDifficulty.Medium });

        Levels.Add(new Level { Name = "First", ImageName = "first.jpg", Difficulty = LevelDifficulty.Hard });
        Levels.Add(new Level { Name = "Cheetah", ImageName = "cheetah.jpg", Difficulty = LevelDifficulty.Hard });
        Levels.Add(new Level { Name = "Koala", ImageName = "koala.jpg", Difficulty = LevelDifficulty.Hard });
        Levels.Add(new Level { Name = "Giraffe", ImageName = "giraffe.jpg", Difficulty = LevelDifficulty.Hard });
    }
}
