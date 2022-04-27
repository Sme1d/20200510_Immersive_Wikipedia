using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RayCastingScript : MonoBehaviour
{
    private Material originalMaterial;
    private Transform selectedObject;
    private Rigidbody grabbedObject;
    private bool editIsActive = false;

    public LineRenderer rayRenderer;
    public Material highlightMaterial;

    public void Start(){
        rayRenderer.material.color = Color.red;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            if (editIsActive){
                rayRenderer.material.color = Color.red;
                resetEditing();
            }
            else{
                rayRenderer.material.color = Color.blue;
                reseHighlighting();
            }
            editIsActive = !editIsActive;
        }

        if (selectedObject && !editIsActive){
            reseHighlighting();
        }

        RaycastHit hit;
        rayRenderer.SetPosition(0, Camera.main.transform.position - Camera.main.transform.up);
        rayRenderer.SetPosition(1, Camera.main.transform.position + Camera.main.transform.forward * 20.0f);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit)){
            rayRenderer.SetPosition(1, hit.point);
            if (hit.transform.tag == "Selectable"){
                if (editIsActive){
                    if (Input.GetMouseButtonDown(0) && !grabbedObject){
                        grabbedObject = hit.rigidbody;
                        grabbedObject.transform.SetParent(gameObject.transform);
                        grabbedObject.isKinematic = true;
                    }
                    else if (Input.GetMouseButtonUp(0) && grabbedObject){
                        resetEditing();
                    }
                }
                else{
                    selectedObject = hit.transform;
                    changeMaterial(highlightMaterial);
                }
            }
        }
    }

    private void reseHighlighting(){
        if (selectedObject){
            changeMaterial(originalMaterial);
            selectedObject = null;
        }
    }

    private void resetEditing(){
        if (grabbedObject){
            grabbedObject.transform.parent = null;
            grabbedObject.isKinematic = false;
            grabbedObject = null;
        }
    }

    private void changeMaterial(Material m){
        var selectionRenderer = selectedObject.GetComponent<Renderer>();
        if (selectionRenderer != null){
            if (m != originalMaterial){
                originalMaterial = selectionRenderer.material;
            }
            selectionRenderer.material = m;
        }
    }
}
