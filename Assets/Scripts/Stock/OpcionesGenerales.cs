using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpcionesGenerales : MonoBehaviour {
    
    public CambiarEscena CambiarEscenaScript;

    public GameObject panelOpcionesGenerales;

    public void MostrarOpcionesGenerales(){

        panelOpcionesGenerales.SetActive(true);

        Transform botonesContainer = panelOpcionesGenerales.transform.GetChild(0).transform;

        botonesContainer.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        botonesContainer.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        botonesContainer.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

        botonesContainer.GetChild(0).GetComponent<Button>().onClick.AddListener(CrearProducto);
        botonesContainer.GetChild(1).GetComponent<Button>().onClick.AddListener(VerRegistros);
        botonesContainer.GetChild(2).GetComponent<Button>().onClick.AddListener(VolverAlInicio);

    }

    public void CerrarPanelOpcionesGenerales(){
        panelOpcionesGenerales.SetActive(false);
    }

    private void CrearProducto(){
        Debug.Log("Hola, aca crearia un producto nuevo...");
    }

    private void VerRegistros(){
        Debug.Log("Aca veria los registros...");
    }

    private void VolverAlInicio(){
        CambiarEscenaScript.CambiarEscenaA("Fichaje");
    }

    

}
