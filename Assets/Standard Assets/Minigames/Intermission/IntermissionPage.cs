﻿using Components;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
  class IntermissionPage : MonoBehaviour
  {
    public Text CountdownText;
    public Text PlayersScoreText;

    private void OnEnable()
    {
      var playersData = GameManager.Instance.GetPlayersData();

      PlayersScoreText.text = string.Empty;

      for (int i = 0; i < GameManager.Instance.GetCurrentPlayersCount(); i++)
      {
        var color = ColorUtility.ToHtmlStringRGB(playersData[i].PlayerColor);
        PlayersScoreText.text += $"<color=\"#{color}\">player {i} has {playersData[i].GameStateData.TotalScore} point(s)</color>\n";
      }
    }
  }
}
