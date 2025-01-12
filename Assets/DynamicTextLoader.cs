using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicTextLoader : MonoBehaviour
{
    TextMeshProUGUI _textField;

    GameObject _linkedComputer;

    List<string> previous = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        _textField = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_linkedComputer != null && gameObject.activeInHierarchy)
        {
            if (previous == null || _linkedComputer.GetComponent<Computer>().events.Count != previous.Count)
            {
                Debug.Log("Updating text");
                previous = _linkedComputer.GetComponent<Computer>().events;

                _textField.text = "antivirus log\n(Yeah)";
                foreach (string s in previous)
                {
                    _textField.text += "\n" + s;
                }
            }
        }
    }

    public void openForComputer(GameObject computer)
    {
        if (_linkedComputer != computer) previous = null;
        _linkedComputer = computer;
        
    }
}
