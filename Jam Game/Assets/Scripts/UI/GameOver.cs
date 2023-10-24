using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI distanceTraveled;
    [SerializeField] private int resetSceneIndex = 0;

    private bool set = false;
    private void Update()
    {
        if (ShipState.Instance.shipIntegrity <= 0)
        {
            if (!set)
            {
                distanceTraveled.text = ShipState.Instance.distanceTraveled.ToString(CultureInfo.InvariantCulture);

            }
            set = true;
            animator.SetTrigger("GameOver");

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(resetSceneIndex);
            }
        }
    }
}
