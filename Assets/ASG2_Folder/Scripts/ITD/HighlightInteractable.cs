using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnHover()
    {
        //Look through all children and store any Mesh Renderers in the meshRenderers array
        //Get all the renderers of this object.
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Use a foreach loop to iterate through the meshRenderers array
        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Enables the Emission property of the renderer's material
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    public void ExitHover()
    {
        //Look through all children and store any Mesh Renderers in the meshRenderers array
        //Get all the renderers of this object.
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Use a foreach loop to iterate through the meshRenderers array
        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Enables the Emission property of the renderer's material
            renderer.material.DisableKeyword("_EMISSION");
        }
    }

    public void OnSelectEnter()
    {
        Debug.Log("vegatable is in placed");
    }
}