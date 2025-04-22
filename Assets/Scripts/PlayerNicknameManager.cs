using System.Collections.Generic;
using Fusion;

public static class PlayerNicknameManager
{
    private static Dictionary<PlayerRef, string> nicknames = new Dictionary<PlayerRef, string>();

    public static void SetNickname(PlayerRef player, string nickname)
    {
        nicknames[player] = nickname;
    }

    public static string GetNickname(PlayerRef player)
    {
        if (nicknames.TryGetValue(player, out string nickname))
            return nickname;
        return $"Player {player.PlayerId}";
    }
}
