/******************************************************************************
 * File         : pLab_MaterialCHanger.cs            
 * Lisence      : BSD 3-Clause License
 * Copyright    : Lapland University of Applied Sciences
 * Authors      : 
 * BSD 3-Clause License
 *
 * Copyright (c) 2019, Lapland University of Applied Sciences
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *  list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *  this list of conditions and the following disclaimer in the documentation
 *  and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its
 *  contributors may be used to endorse or promote products derived from
 *  this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pLab_GpsPosition : MonoBehaviour
{



    public static pLab_GpsPosition Instance { get; set; }



    public float Latitude;
    public float Longitude;


    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());

    }


private IEnumerator StartLocationService()
    {

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("GPS is not active!");
            Debug.Log("eitoimi");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1f);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.LogWarning("Timed Out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogWarning("Unable to determine device location");
            yield break;
        }

        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;
        

        Debug.Log("latitude" + Latitude);
        Debug.Log("longitude " + Longitude);
        yield break;

        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(40, 40, 200, 90), "Map selected: " + Latitude + " ja " + Longitude);
    }



}
















/*IEnumerator Start()
{
    // First, check if user has location service enabled
    if (!Input.location.isEnabledByUser)
        Debug.Log("status on" +Input.location.status);
        //yield break;

    // Start service before querying location
    Input.location.Start(10,0.1f);

    // Wait until service initializes
    int maxWait = 20;
    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    {
        yield return new WaitForSeconds(1);
        maxWait--;
    }

    // Service didn't initialize in 20 seconds
    if (maxWait < 1)
    {
        print("Timed out");
        yield break;
    }

    // Connection has failed
    if (Input.location.status == LocationServiceStatus.Failed)
    {
        print("Unable to determine device location");
        yield break;
    }
    else
    {
        // Access granted and location value could be retrieved
        print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
    }

    // Stop service if there is no need to query location updates continuously
    Input.location.Stop();
}

public void Update()
{
    if (Input.location.isEnabledByUser){
        float lat = Input.location.lastData.latitude;
        float lon = Input.location.lastData.longitude;

        Debug.Log("sijainti lat on " + lat);
        Debug.Log("sijainti lon on " + lon);
    }
}

}






public float lat, lon;


IEnumerator start(){
 while (!Input.location.isEnabledByUser){

     yield return new WaitForSeconds(1f);

 }

 Input.location.Start(10f, 5f);

 int maxWait = 20;
 while(Input.location.status  == LocationServiceStatus.Initializing && maxWait > 0){

     yield return new WaitForSeconds(1f);
     maxWait--;
 }

 if(maxWait <1){
     yield break;
 }

 if(Input.location.status == LocationServiceStatus.Failed){
     yield break;
 }

 else{
     lat = Input.location.lastData.latitude;
     lon = Input.location.lastData.longitude;
 }

 while (Input.location.isEnabledByUser){
     yield return new WaitForSeconds(1f);
 }

}

// Update is called once per frame
void Update()
{
 Debug.Log("tilanne on" + lat);
 Debug.Log("talanne on" + lon);
}
}*/
