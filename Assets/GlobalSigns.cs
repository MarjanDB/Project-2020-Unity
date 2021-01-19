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
        /*
        StartCoroutine(makeRequest("http://192.168.1.10:3000/api/getLocations"));
        */
        signs.Add(new Signs { latitude = 46.49339,
            longitude = 15.6030694, type = "STOP" });

        
        foreach(Signs x in signs)
        {
            GameObject sign = null;
            bool valid = true;
            string resource = "";
            switch (x.type)
            {
                case "stop":
                    resource = ("Signs/STOP");
                    break;
                case "prednostna":
                    resource = ("Signs/PREDNOSTNA");
                    break;
                case "odvzem_prednostne":
                    resource = ("Signs/ODVZEM");
                    break;
                default:
                    valid = false;
                    break;
            }


            if (valid)
            {
                Vector3 pos = new Vector3((float)x.longitude, (float)x.latitude);
                sign.transform.position = pos;
                Instantiate(Resources.Load(resource), pos, Camera.main.transform.rotation);
            }
        }
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
