using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image barImage;

    void Start()
    {
        player.OnHealthUpdate += Player_OnHealthUpdate; 
        barImage.fillAmount = 1f;
    }

    private void Player_OnHealthUpdate(object sender, Player.OnHealthUpdateEventArgs e)
    {
        barImage.fillAmount = e.playerCurrentHealthNormalized;
    }

}
