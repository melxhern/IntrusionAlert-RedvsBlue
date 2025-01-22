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
    public TMP_Text Postit;
    public GameObject Panel;
    
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
        //Panel.SetActive(false);
        ThirdPersonController player = ThirdPersonController.Local;
        RoleText.text = $"You are {player.Role.ToString().ToLower()}";

        StartCoroutine(DoFade());

        StartCoroutine(WaitAndShowPanel(4f));

        
       // Panel.SetActive(true);


        if( player.Role.ToString() == "RedTeam")  Postit.text="Votre rôle protéger le réseau interne, \nvotre mission vérifier et mettre en place les firewalls";

        if( player.Role.ToString() == "BlueTeam")  Postit.text="Votre rôle entrer dans le réseau interne, \nvotre mission inserer dans les ordinateurs non protéger les clés USB inféctées.";
      
        
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

    private IEnumerator WaitAndShowPanel(float waitTime)
{
    yield return new WaitForSeconds(waitTime);
    if (Panel != null) Panel.SetActive(true);
}
}
