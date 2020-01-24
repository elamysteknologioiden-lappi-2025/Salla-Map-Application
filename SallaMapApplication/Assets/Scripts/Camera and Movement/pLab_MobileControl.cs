/******************************************************************************
 * File         : pLab_MobileControl.cs            
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

public class pLab_MobileControl : MonoBehaviour{

    //The player object which the camera will follow.
    [SerializeField]
    private GameObject followObject;

    [SerializeField]
    private Camera MainCamera;

    Vector2 deltaPosition;

    //Camera settings 
    private float maxZoom;
    private float orthoCamSize;
    private float orthoZoomSpeed = 0.25f;
    private float minZoom = 300;
    private float cameraHeight = 150f;

    
    private GameObject infoScreen;
    private Rigidbody cameraRestriction;

    

    void Start(){

        deltaPosition = Input.mousePosition;

        maxZoom = Camera.main.orthographicSize;
        orthoCamSize = maxZoom;


        infoScreen = GameObject.Find("Scroll View");
        

    }

    
    void Update(){
       
        if (!infoScreen.activeSelf){

            //The "if" below is for rotating the camera around the player
            if (Input.touchCount == 1){


                Touch touch = Input.GetTouch(0);

                Vector2 touchZeroPrevPos = touch.position - touch.deltaPosition;

                Vector2 screenPos = MainCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);

                Vector3 prevLo = touchZeroPrevPos - screenPos;
                Vector3 nowLo = touch.position - screenPos;
                Vector3 aa = transform.transform.localEulerAngles;

                float angle = Vector3.SignedAngle(prevLo, nowLo, Vector3.forward);
                float sh = Screen.height;
                float sw = Screen.width;
                float mmm = touch.deltaPosition.magnitude;

                aa.y += angle;
                transform.transform.localEulerAngles = aa;
            }


            //The "if" below is for zooming the camera object (orthographic in this case.)
            if (Input.touchCount == 2){

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                MainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                ///Next two lines make sure the zoom never goes beyond given limits.
                MainCamera.orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, minZoom);
                MainCamera.orthographicSize = Mathf.Min(GetComponent<Camera>().orthographicSize, maxZoom);

            }

            Vector3 pos = followObject.transform.position;
            pos.y += cameraHeight;
            transform.position = pos;
          
        }

        //For rotating 
        else{

            cameraRestriction.constraints = RigidbodyConstraints.FreezeRotationZ;

        }
    }
}
 