using UnityEngine;
using UnityEngine.UI;
using System;

public class OpcionesGenerales : MonoBehaviour {

    public ConexionDB DBScript;    
    public CambiarEscena CambiarEscenaScript;

    public GameObject panelOpcionesGenerales;

    public void MostrarOpcionesGenerales(){

        panelOpcionesGenerales.SetActive(true);

        Transform botonesContainer = panelOpcionesGenerales.transform.GetChild(0).transform;

        botonesContainer.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        botonesContainer.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        botonesContainer.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

        botonesContainer.GetChild(0).GetComponent<Button>().onClick.AddListener(MostrarPanelCrearProducto);
        botonesContainer.GetChild(1).GetComponent<Button>().onClick.AddListener(VerRegistros);
        botonesContainer.GetChild(2).GetComponent<Button>().onClick.AddListener(VolverAlInicio);

    }

    public void CerrarPanelOpcionesGenerales(){
        panelOpcionesGenerales.SetActive(false);
    }

    #region Opción: Crear Producto

    public GameObject panelCrearProducto;
    public InputField codigoIF;
    public InputField nombreProductoIF;
    public InputField precioUnitarioIF;
    public InputField promocionIF;
    public InputField stockIF;
    public InputField unidadesVendidasIF;
    public InputField fechaCreacionIF;
    public InputField proveedorIF;
    public Button crearProductoBtn;

    private void MostrarPanelCrearProducto(){

        CerrarPanelOpcionesGenerales();

        GameObject ifsContainer = codigoIF.transform.parent.parent.gameObject;
        
        panelCrearProducto.SetActive(true);

        crearProductoBtn.onClick.RemoveAllListeners();
        crearProductoBtn.onClick.AddListener(CrearProducto);

    }

    private void CrearProducto(){

        DBScript.AbrirDB();
        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        string sqlQuery = String.Format("INSERT INTO Mercaderia (codigo, producto, 'precio unitario', promocion, stock, 'unidades vendidas', 'fecha de creacion', proveedor) VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\")", codigoIF.text, nombreProductoIF.text, precioUnitarioIF.text, promocionIF.text, stockIF.text, unidadesVendidasIF.text, fechaCreacionIF.text, proveedorIF.text);  
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        panelCrearProducto.SetActive(false);

        CambiarEscenaScript.CambiarEscenaA("Mercaderia");
    }

    public void CerrarPanelCrearProducto(){
        panelCrearProducto.SetActive(false);
    }

    #endregion

    private void VerRegistros(){
        Debug.Log("Aca veria los registros...");
    }

    private void VolverAlInicio(){
        CambiarEscenaScript.CambiarEscenaA("Fichaje");
    }

    

}
