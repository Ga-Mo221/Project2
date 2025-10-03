using System.Collections.Generic;

public static class PlayerTag
{
    private static readonly HashSet<string> _playerTags = new HashSet<string>
    { "Warrior", "Archer", "Lancer", "TNT", "Healer" };

    private static readonly HashSet<string> _playerFarm = new HashSet<string>
    { "Warrior", "Archer", "Lancer"};

    public static bool checkTag(string tag)
    {
        return _playerTags.Contains(tag);
    }

    public static bool checkTagFarm(string tag)
    {
        return _playerFarm.Contains(tag);
    }
}
