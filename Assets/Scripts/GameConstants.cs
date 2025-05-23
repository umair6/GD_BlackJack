using UnityEngine;

public static class GameConstants
{

    public const float CARD_SPACING = 0.27f;
    public const float CARD_MOVE_DELAY = 0.5f;
    public const float CARD_FLIP_DELAY = 0.1f;



    public static float GetCardMoveDelay()
    {
        return CARD_MOVE_DELAY + CARD_FLIP_DELAY + CARD_FLIP_DELAY;

    }

}
