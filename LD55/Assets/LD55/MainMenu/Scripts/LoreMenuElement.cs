using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoreMenuElement : MonoBehaviour
{
    public Material mouseOverMaterial;
    public GameObject MenuText;
    public string MeshRendererTag;
    public string DisableObjectTag;
    List<MeshRenderer> meshRenderers;
    List<GameObject> objectToDisable;

    private void Awake()
    {
        meshRenderers = GameObject.FindGameObjectsWithTag(MeshRendererTag).Select(s => s.GetComponent<MeshRenderer>()).ToList();
        objectToDisable = GameObject.FindGameObjectsWithTag(DisableObjectTag).ToList();
        objectToDisable.Add(MenuText);

        foreach(var light in objectToDisable)
        {
            light.SetActive(false); 
        }
    }

    private void OnMouseEnter()
    {
        foreach(var book in meshRenderers)
        {
            Material[] matArray = book.materials;
            var newMaterialArray = new Material[2];
            newMaterialArray[0] = matArray[0];
            newMaterialArray[1] = mouseOverMaterial;
            book.materials = newMaterialArray;
        }

        foreach (var light in objectToDisable)
        {
            light.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        foreach (var book in meshRenderers)
        {
            Material[] matArray = book.materials;
            var newMaterialArray = new Material[1];
            newMaterialArray[0] = matArray[0];
            book.materials = newMaterialArray;
        }

        foreach (var light in objectToDisable)
        {
            light.SetActive(false);
        }
    }
}