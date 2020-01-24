/******************************************************************************
 * File         : pLab_GpsPosition.cs         
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class pLab_GpsPosition : MonoBehaviour{

    /// <summary>
    /// Lines  51-62 are for getting variables from device location and compass direction
    /// </summary>
    public static pLab_GpsPosition Instance { get; set; }

    private double utmX = 0;
    public double UtmX { get { return this.utmX; } }


    private double utmY = 0;
    public double UtmY { get { return this.utmY; } }


    private float compassDir = 0;
    public float CompassDir { get { return this.compassDir; } }

    
    /// <summary>
    /// Lines 68-91 variables are for calculating the POI-object distances and movement of player
    /// </summary>
    private float distance = 0;
    private float timeDif = 0;
    private float startTime;
    private float speed = 0;

    private float shortestDistance;

    private float filteredHeading = 0;
    private float headingSmoothVelocity = 0;
    private float headingSmooth = 0.3f;

    private float time;

    private float distanceToPoi;

    private float dSqrToTarget;

    [SerializeField] private float minX, maxX, minY, MaxY;

    private Vector3 newPos;
    private Vector3 startPosition, changePos, desiredPos, lastPos = Vector3.zero;

    private double lastHeadingTimeStamp = 0;

    /// <summary>
    /// Lines  96-116 are for getting the POI-objects values and setting them up for gameplay
    /// </summary>
    [SerializeField] Text poiName, poiDesc;

    [SerializeField] GameObject WelcomeScreen, InfoScreen, PauseScreen, EnableButton, PauseButton;
    private GameObject mainCamera;

    private GameObject ClosestPoi;



    [SerializeField] Image imageShower;
    private Image imageGiver;

    
    private string name, description;


    [SerializeField] GameObject outOfRange;
    private Boolean isPaused = false;
        

  GameObject[] AllPois;


    /// <summary>
    /// In start We check if the device GPS is turned on, setup player location, setup UI-elements and find all POI-objects.
    /// </summary>
    void Start(){
        
        shortestDistance = Mathf.Infinity;

        StartCoroutine(StartLocationService());
        
        desiredPos = transform.position;

        InfoScreen.SetActive(false);

        AllPois = GameObject.FindGameObjectsWithTag("PoiObject");
 
        PauseScreen.SetActive(false);
        EnableButton.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(true);
            
    }

    /// <summary>
    /// Following code obtains the device location and sets it up, and if it won't get one, gives out yield.
    /// </summary>
    private IEnumerator StartLocationService(){

        if (!Input.location.isEnabledByUser){
            yield break;
        }

        Input.compass.enabled = true;

        Input.location.Start(1,1f);

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0){
            yield return new WaitForSeconds(1f);
            maxWait--;
        }

        if (maxWait <= 0){
            yield break;
        }
       
        yield break;
    }
    
    /// <summary>
    /// Here we will go through more important elements one by one. Overall the update looks for the closest POI-object.
    /// </summary>
    private void Update(){

        //Following is transforming GPS-data for unity and putting it in a variable.
        pLAB_GeoUtils.LatLongtoUTM((double)Input.location.lastData.latitude, (double)Input.location.lastData.longitude,
            out this.utmY, out this.utmX);
        
        //Checking if player's GPS-data area is out of play area. If so, ingame the player will be notified of this.
        if((utmX < minX) || (utmX > maxX) && (utmY < minY) || (utmY > MaxY)) {

            outOfRange.gameObject.SetActive(true);
           
        }

        /// <summary>
        /// If GPS-location is found, the players location will be updated constantly with the data, in the end FindClosestPoi
        /// is called for finding the closest POI
        /// </summary>
        else
        {
 
            outOfRange.gameObject.SetActive(false);

            if (startPosition == Vector3.zero){

                startPosition = transform.position;
            }

            if (desiredPos == Vector3.zero){

                desiredPos = transform.position;
            }

            if (transform.position == startPosition){

                transform.position = new Vector3(desiredPos.x, 0.4f, desiredPos.z);
            }

            newPos = new Vector3((float)(UtmX - pLab_mapStartPoint.instance.UtmX),
            4f, (float)(UtmY - pLab_mapStartPoint.instance.UtmY));

            if (Vector3.Distance(newPos, desiredPos) > 0.1f) {

                distance = Vector3.Distance(newPos, transform.position);

                timeDif = Time.time - startTime;

                if (timeDif > 0.5f){

                    timeDif = 0.5f;
                }

                startTime = Time.time;

                desiredPos = newPos;
                time = 0;

            }


            speed = distance / timeDif / 60;

            time += Time.deltaTime;

            double timestamp = Input.compass.timestamp;

            if (Vector3.Distance(transform.position, desiredPos) > 1f) {
                transform.LookAt(desiredPos);
                transform.rotation *= Quaternion.Euler(90, 0, 0);
                transform.position = Vector3.MoveTowards(lastPos, desiredPos, speed);
                
            }

            else if (Input.compass.enabled && timestamp > lastHeadingTimeStamp){

                lastHeadingTimeStamp = timestamp;
                CalculateFilteredHeading(Input.compass.trueHeading);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, filteredHeading, 0), 1f); 
                
            }



            Vector3 targetDir = newPos;
            lastPos = transform.position;

            FindClosestPoi();

        }
    }

    /// <summary>
    /// This function Goes through the POI objects, looks at their distance to the player and saves the closest.
    /// If the closest is within given reach, the player is given the option to view that POI:s info with UI-elements.
    /// </summary>
    void FindClosestPoi(){

        AllPois = GameObject.FindGameObjectsWithTag("PoiObject");
        shortestDistance = Mathf.Infinity;


        foreach(GameObject currentClosest in AllPois){

            Vector3 directionToTarget = currentClosest.transform.position - this.transform.position;
            dSqrToTarget = directionToTarget.sqrMagnitude;

            if(dSqrToTarget < shortestDistance){

                ClosestPoi = null;
                shortestDistance = dSqrToTarget;
                ClosestPoi = currentClosest;
            }

            if (shortestDistance < 7500) {

                EnableButton.gameObject.SetActive(true);
            }

            else {

                EnableButton.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// When player is in range, script enables "enablebutton" which in turn when pressed, calls the following function.
    /// This function sets the closest POI:s values to the UI-elements. CanvasMaster - PoiInfoHandler - enableButton
    /// </summary>
    public void IfInRange(){
        
        InfoScreen.SetActive(true);

       //name = ClosestPoi.GetComponent<pLab_PoiData>().newName;
        description = ClosestPoi.GetComponent<pLab_PoiData>().description;
        imageGiver = ClosestPoi.GetComponent<Image>();

        poiName.text = name;
        poiDesc.text = description;
        imageShower.sprite = imageGiver.overrideSprite;

    }

   
    /// <summary>
    /// When player closes the POI-UI the values will be reseted to make sure that no funny business happens, and also hides
    /// the UI. This function is attached to "disableButton". CanvasMaster - PoiInfoHandler - Scroll View - disableButton
    /// </summary>
    public void hidePoi(){

        poiName.text = null;
        poiDesc.text = null;
        imageGiver = null;
        InfoScreen.SetActive(false);

    }

    /// <summary>
    /// Attached to the "BeginButton" which shows up when loading play-scene. Gives the player-model time to get into the right
    /// position. Canvasmaster - WelcomeScreen - BeginButton
    /// </summary>
    public void HideWelcome(){

        WelcomeScreen.gameObject.SetActive(false);

    }

    /// <summary>
    /// For pause function of the game which lets the player return to main menu and from there to change the scene.
    /// CanvasMaster - pauseMenu - pauseButton
    /// </summary>
    public void ActivatePause(){

        if(PauseScreen.gameObject.activeInHierarchy == false){

            PauseScreen.gameObject.SetActive(true);
        }

        else{

            PauseScreen.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// For calculating the position in which the player should pointing with the device.
    /// </summary>
    private void CalculateFilteredHeading(float rawCompassHeading){

        filteredHeading = Mathf.SmoothDampAngle(filteredHeading, rawCompassHeading, ref headingSmoothVelocity, headingSmooth);
    }


   /*This is purely for testing on mobile devices.
    void OnGUI(){

        GUI.Label(new Rect(400, 400, 400, 1000), "text here" + variable + " moretext " + anotherVariable);

    }*/

}