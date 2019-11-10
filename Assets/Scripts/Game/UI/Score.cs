using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  [SerializeField]
  private Text text;

  private void Start()
  {
    EventManager.StartListening(EventManager.Events.DotsDestroyed, onDotDestroyed);
    updateScoreText();
  }

  private void onDotDestroyed()
  {
    updateScoreText();
  }

  private void updateScoreText()
  {
    text.text = PlayerData.Score.ToString();
  }
}
