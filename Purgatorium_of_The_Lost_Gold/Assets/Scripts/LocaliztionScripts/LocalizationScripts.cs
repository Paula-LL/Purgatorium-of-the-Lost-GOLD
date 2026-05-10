using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationScripts : MonoBehaviour
{
    [SerializeField]
    string lang;


    public void NewLocale(string newLang)
    {


        Debug.Log("Entered function");
        switch (newLang)
        {

            case "ca":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                Debug.Log("Entered Catalan: " + LocalizationSettings.AvailableLocales.Locales[0]);
                break;

            case "en":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                Debug.Log("Entered English: " + LocalizationSettings.AvailableLocales.Locales[1]);
                break;

            case "es":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
                Debug.Log("Entered Spanish: " + LocalizationSettings.AvailableLocales.Locales[2]);
                break;

            default:
                break;


        }
        lang = newLang;
        Debug.Log("Left Switch");

    }


}
