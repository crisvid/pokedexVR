using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class readJson : MonoBehaviour
{
    public string jsonUrl;
    public TextMeshProUGUI information;

    public void getPokemon()
    {
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        WWW _www = new WWW(jsonUrl);
        yield return _www;

        if(_www.error == null)
        {
            Pokemon readPokemon = JsonUtility.FromJson<Pokemon>(_www.text);

            information.text = readPokemon.ToString();

        }
        else
        {
            Debug.Log("Error");
        }
    }
}

