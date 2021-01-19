using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GlobalSigns : MonoBehaviour
{
    public GPS GPSController;

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

    private void position(LocationInfo location, ref Signs sign)
    {
        double deltaLatitude = sign.latitude - location.latitude;
        double deltaLongitude = sign.longitude - location.longitude;

        sign.latitude = deltaLatitude * 40008000 / 360.0f;
        sign.longitude = deltaLongitude * 40075160 * Math.Cos(sign.latitude * Mathf.PI / 180) / 360;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        /*
        StartCoroutine(makeRequest("http://192.168.1.10:3000/api/getLocations"));
        */

        //for testing purposes, place in same position
        signs.Add(new Signs { latitude = GPSController.Location.latitude,
            longitude = GPSController.Location.longitude, type = "STOP" });

        while (!GPSController.isUpdating) yield return new WaitForSeconds(1);
        
        foreach(Signs sign in signs)
        {
            string resource = "";

            switch (sign.type)
            {
                case "stop":
                    resource = "Signs/STOP";
                    break;
                case "prednostna":
                    resource = "Signs/PREDNOSTNA";
                    break;
                case "odvzem_prednostne":
                    resource = "Signs/ODVZEM";
                    break;
            }

            Signs s = sign;
            position(GPSController.Location, ref s);

            Vector3 pos = new Vector3((float)s.longitude, (float)s.latitude);
            Instantiate(Resources.Load(resource), pos, Camera.main.transform.rotation);

        }
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
