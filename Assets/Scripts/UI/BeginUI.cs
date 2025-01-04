using Assets.Scripts;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeginUI : MonoBehaviour
{
    public CanvasGroup Group;
    public TMP_Text RoleText;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnGameStarted += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStarted -= OnGameStart;
    }

    private void OnGameStart()
    {
        ThirdPersonController player = ThirdPersonController.Local;
        RoleText.text = $"You are {player.Role.ToString().ToLower()}";

        StartCoroutine(DoFade());
    }

    private IEnumerator DoFade()
    {
        yield return new WaitForSeconds(2);

        float time = 0, duration = 2;
        while (time < duration)
        {
            Group.alpha = Mathf.Lerp(1, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Group.alpha = 0;
        Group.blocksRaycasts = false;
        Group.interactable = false;
    }
}
