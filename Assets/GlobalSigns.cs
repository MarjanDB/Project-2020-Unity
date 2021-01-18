using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalSigns : MonoBehaviour
{

    [System.Serializable]
    public class Signs
    {
        public double latitude;
        public double longitude;
        public string type;
    }


    [System.Serializable]
    public class SignWrapper
    {
        public Signs[] data;
    }


    public List<Signs> signs = new List<Signs>();



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeRequest("http://192.168.1.10:3000/api/getLocations"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator makeRequest(string uri)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(uri))
        {
            yield return req.SendWebRequest();


            if(req.isNetworkError)
            {
                Debug.Log(req.error);
            } else
            {
                string res = req.downloadHandler.text;
                Debug.Log(res);
                Signs[] l = JsonUtility.FromJson<SignWrapper>(res).data;
                signs.AddRange(l);
            }


        }
    }
}
