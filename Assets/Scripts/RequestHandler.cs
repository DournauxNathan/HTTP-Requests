using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RequestHandler : MonoBehaviour
{
    [SerializeField] private string url = "";
    [SerializeField] private string jsonData = "";
    [SerializeField] private string urlToUpdate = "";
    [SerializeField] private string urlToDelete = "";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakeRequest());
        /*StartCoroutine(PostRequest());
        StartCoroutine(PutRequest());
        StartCoroutine(DeleteRequest());*/
    }

    IEnumerator MakeRequest()
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
                Debug.Log("Response: " + response);

                JSONPlaceHolder placeHolder = JsonUtility.FromJson<JSONPlaceHolder>(response);
                Debug.Log(placeHolder.userID);
                Debug.Log(placeHolder.id);
                Debug.Log(placeHolder.title);
                Debug.Log(placeHolder.body);

                /*Pokemon pokemon = JsonUtility.FromJson<Pokemon>(response);
                Debug.LogError(pokemon.name);*/
            }
        }
    }

    IEnumerator PostRequest()
    {
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
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
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(urlToUpdate);
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
        UnityWebRequest webRequest = new UnityWebRequest(urlToDelete, "DELETE");
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
}

[System.Serializable]
public class Pokemon 
{
    public int id;
    public Sprite sprites;
    public string name;
    public float heigt;
    public float weight;
}

[System.Serializable]
public class JSONPlaceHolder
{
    public int userID;
    public int id;
    public string title;
    public string body;
}

