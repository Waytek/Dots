using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
  private static GameSettings instance;
  public static GameSettings Instance
  {
    get
    {
      if (!instance)
      {
        instance = FindObjectOfType(typeof(GameSettings)) as GameSettings;

        if (!instance)
        {
          instance = new GameObject("GameSettings").AddComponent<GameSettings>();
        }
      }

      return instance;
    }
  }

  [SerializeField]
  private int scoreFromDot = 10;
  public static int ScoreFromDot => Instance.scoreFromDot;
}
