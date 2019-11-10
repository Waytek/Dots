using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
  public static int Score
  {
    get
    {
      return PlayerPrefs.GetInt("playerScore", 0);
    }
    set
    {
      PlayerPrefs.SetInt("playerScore", value);
    }
  }
}
