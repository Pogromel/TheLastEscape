using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    
    [SerializeField] private GameObject prompt;
    [SerializeField] private TextMeshProUGUI promptText;
    
    
    public bool SetInteractionPrompt(string text)
    {
        prompt.SetActive(text!=String.Empty);
        promptText.text = text;
        return text!=String.Empty;
    }
}
