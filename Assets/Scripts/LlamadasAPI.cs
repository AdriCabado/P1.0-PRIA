/*

Tired of creating tons of classes for parsing a simple JSON call? 

Simple JSON é unha librería externa que permite acceder de forma dinámica e sen crear as clases que representan o obxecto json.
https://github.com/Bunny83/SimpleJSON

Para usar a clase, hai este código de exemplo. Asegúrate de descargar o código simplejson.cs e incluilo no proxecto (como está feito neste exemplo).

*/


using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;

public class LlamadasAPI : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetDataCoroutine("https://opentdb.com/api.php?amount=10&type=boolean"));
    }

    IEnumerator GetDataCoroutine(string apiUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var N = JSON.JSON.Parse(request.downloadHandler.text);
            Debug.Log(N.ToString());
            // Now you can access your data dynamically. For example:
            string value = N["results"][0]["type"].Value; // Now you can access JSON like in javascript with keys and 
            Debug.Log(value);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
