/******************************************************************************
 * File         : pLab_InspectorTool.cs            
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
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// Editor tool which lets user to spawn new POI-object to the editor to the right place without copying existing objects or
/// prefabs. Called "Poi Tool" in the menus on top of unity screen. Values related to the POI need to be set in the created object.
/// Poi Objects are found under "PoiMaster" and are categorized to few underclasses
/// </summary>

#if UNITY_EDITOR
[CustomEditor(typeof(pLab_PoiData))]

public class pLab_InspectorTool : EditorWindow {

    [MenuItem("PoI Tool/Create Point of interest")]

    static void CreatePoi(){

        GameObject TestPoi = Instantiate(Resources.Load("Prefabs/poiBase", typeof(GameObject))) as GameObject;
        TestPoi.transform.parent = GameObject.Find("PoiMaster").transform;
        TestPoi.transform.position = new Vector3(0,6,0);

    }

    [MenuItem("poiBase", true)]
    static bool ValidateInstantiatePrefab(){

        GameObject go = Selection.activeObject as GameObject;
        if(go == null)

            return false;
        

        return PrefabUtility.IsPartOfPrefabAsset(go);        

    }

}
#endif



