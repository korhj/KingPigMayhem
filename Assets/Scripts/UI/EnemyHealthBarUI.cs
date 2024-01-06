using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour {

    [SerializeField] private Petrifex enemy;
    [SerializeField] private Image barImage;

    void Start()
    {
        enemy.OnHealthUpdate += Enemy_OnHealthUpdate; 
        barImage.fillAmount = 1f;
    }

    private void Enemy_OnHealthUpdate(object sender, IEnemy.OnHealthUpdateEventArgs e)
    {
        barImage.fillAmount = e.enemyCurrentHealthNormalized;
    }

}
