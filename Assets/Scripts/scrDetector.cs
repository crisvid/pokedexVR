﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Google.Protobuf;
using Newtonsoft.Json;
using UnityEngine.Rendering;
using JetBrains.Annotations;

public class scrDetector : MonoBehaviour
{
    public RawImage image;
    public float probabiltyThreshold = 0.35f;
    public GameObject boxFactory;
    private Texture2D texture;
    public GameObject FileManager;
    public int mode = 1; //mode 0 no image, 1 galeria , 2 camara
    public readJson infoPokemon;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void Analize()
    {
        byte[] bytes;

        
        if(mode == 2)
        {
            texture = this.FileManager.GetComponent<FileManager>().StopWebCamera();
        } 
        else if (mode == 1)
        {
            //texture = image.texture;
            // Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary(
                                image.texture.width,
                                image.texture.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);

            // Blit the pixels on texture to the RenderTexture
            Graphics.Blit(image.texture, tmp);
            // Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;
            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;
            // Create a new readable Texture2D to copy the pixels to it
            texture = new Texture2D(image.texture.width, image.texture.height);
            // Copy the pixels from the RenderTexture to the new Texture
            texture.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            texture.Apply();
            // Reset the active RenderTexture
            RenderTexture.active = previous;
            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);

            // "myTexture2D" now has the same pixels from "texture" and it's readable.
        }
        bytes = texture.EncodeToPNG();
        // Create a Web Form
        StartCoroutine(Upload(bytes));
    }


    IEnumerator Upload(byte[] data)
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post("", form)) // URL aqui
        {
            www.SetRequestHeader("Prediction-Key", ""); //KEY aqui
            www.SetRequestHeader("Content-Type", "application/octet-stream");
            www.uploadHandler = new UploadHandlerRaw(data);
            www.uploadHandler.contentType = "application/octet-stream";

            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;

                AnalysisObject analysisObject = new AnalysisObject();
                analysisObject = JsonConvert.DeserializeObject<AnalysisObject>(jsonResponse);
                //foreach (Prediction predict in analysisObject.Predictions){
                //    if (predict.Probability > 0.1)
                //    {
                //        boxFactory.GetComponent<scrBoxFactory>().CreateBox(predict.boundingBox,predict.TagName, texture.width, texture.height);
                //    }                    
                //}
                if (analysisObject.Predictions[0].Probability > 0.1)
                {
                    //boxFactory.GetComponent<scrBoxFactory>().CreateBox(analysisObject.Predictions[0].boundingBox, analysisObject.Predictions[0].TagName, texture.width, texture.height);
                    
                    infoPokemon.jsonUrl = "https://pokeapi.co/api/v2/pokemon/" + analysisObject.Predictions[0].TagName.ToLower();
                    infoPokemon.getPokemon();
                }
                else
                {
                    infoPokemon.information.text = "No hay datos que mostar";
                }
                
            }
        }
    }
}




