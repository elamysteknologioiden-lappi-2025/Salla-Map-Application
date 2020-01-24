/******************************************************************************
 * File         : pLab_PoiData.cs            
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
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Every single POI-object has this attached to it. Used to save the info about the certain object/Location.
/// </summary>

public class pLab_PoiData : MonoBehaviour{

    [Header("Information")]

   /* [Tooltip("Give the name for identifying the Point of Interest")]
    public string newName;*/

    [Multiline(6)]
    [Tooltip("Here you can input the name and information of the PoI")]
    public string description;


    [Header("Coordinates")]
    [Tooltip("Insert the location of PoI. You can get this information for example from retkikartta.fi Object will not appear without proper coordinates")]
    public float utmx;
    [Tooltip("Insert the location of PoI. You can get this information for example from retkikartta.fi Object will not appear without proper coordinates")]
    public float utmy;

    private Vector3 poiLocation;

    void Start(){

        poiLocation = new Vector3((float)(utmx - pLab_mapStartPoint.instance.UtmX),
        6f, (float)(utmy - pLab_mapStartPoint.instance.UtmY));
        transform.position = poiLocation;

        if (utmx == 0 || utmy == 0){

            Destroy(this);

        }

    }

}