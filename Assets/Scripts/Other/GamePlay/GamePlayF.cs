using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public static class GamePlayF
{

    public static void Pause()
    {
        Time.timeScale = 0;
    }
    public static void Continue()
    {
        Time.timeScale = 1;
    }
}
