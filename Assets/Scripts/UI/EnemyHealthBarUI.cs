using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField]
    private GameObject hasEnemyHealthBarGameObject;

    [SerializeField]
    private Image barImage;

    private IHasEnemyHealthBar hasEnemyHealthBar;

    void Start()
    {
        hasEnemyHealthBar = hasEnemyHealthBarGameObject.GetComponent<IHasEnemyHealthBar>();
        if (hasEnemyHealthBar == null)
        {
            Debug.LogError(
                "GameObject "
                    + hasEnemyHealthBarGameObject.name
                    + "does not implement IHasEnemyHealthBar"
            );
        }
        hasEnemyHealthBar.OnHealthUpdate += HasEnemyHealthBar_OnHealthUpdate;
        barImage.fillAmount = 1f;
    }

    private void HasEnemyHealthBar_OnHealthUpdate(
        object sender,
        IHasEnemyHealthBar.OnHealthUpdateEventArgs e
    )
    {
        barImage.fillAmount = e.enemyCurrentHealthNormalized;
    }
}
