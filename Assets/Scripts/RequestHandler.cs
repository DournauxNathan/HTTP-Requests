using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

#if UNITY_EDITOR
[CustomEditor(typeof(RequestHandler))]
public class HttpRequestManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RequestHandler manager = (RequestHandler)target;

        GUILayout.Space(10);
                
        if (GUILayout.Button("GET"))
        {
            manager.CallCoroutine("GetRequest");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("POST"))
        {
            manager.CallCoroutine("PostRequest");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("PUT"))
        {
            manager.CallCoroutine("PutRequest");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("DELETE"))
        {
            manager.CallCoroutine("DeleteRequest");
        }
    }
}

[ExecuteInEditMode]
#endif
public class RequestHandler : MonoBehaviour
{
    [SerializeField] private string url = "";
    /*[SerializeField]*/ private Post jsonData;

    IEnumerator GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Successfully received a response.
                string response = webRequest.downloadHandler.text;

                JSONPlaceHolder placeHolder = JsonUtility.FromJson<JSONPlaceHolder>(response);
                Debug.Log(placeHolder.userID);
                Debug.Log(placeHolder.id);
                Debug.Log(placeHolder.title);
                Debug.Log(placeHolder.body);
            }
        }
    }

    #region FOR NOW - All the method below CANNOT perform their purposes.
    /* The JSONPlaceholder API provides a read-only, 
     * mock RESTful API, which means you can retrieve data but 
     * cannot actually perform POST, PUT, or DELETE operations.
     * It's designed for practicing and testing HTTP requests 
     * without making any changes to the server.
     * 
     * To practice and develop functionality that involves making 
     * HTTP requests with real POST, PUT, and DELETE operations, 
     * you should use a real API or server that allows such modifications.
     */
    IEnumerator PostRequest()
    {
        string _jsonData = JsonUtility.ToJson(jsonData);

        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(_jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();
                
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            string response = webRequest.downloadHandler.text;
            Debug.Log("Response: " + response);
        }
    }

    IEnumerator PutRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(url);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            string response = webRequest.downloadHandler.text;
            Debug.Log("Response: " + response);
        }
    }

    IEnumerator DeleteRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            string response = webRequest.downloadHandler.text;
            Debug.Log("Response: " + response);
        }
    }

    #endregion
    
    public void CallCoroutine(string method)
    {
        StartCoroutine(method);
    }
}

[System.Serializable]
public class JSONPlaceHolder
{
    public int userID;
    public int id;
    public string title;
    public string body;
}

[System.Serializable]
public class Post
{
    public string title;
    public string body;
}

[System.Serializable]
public class Comment
{
    public int postId;
    public int id;
    public string name;
    public string email;
    public string body;
}
