using UnityEngine;
using UnityEngine.UI;
using System;

public class MostrarMercaderia : MonoBehaviour {

    //Scripts
    public ConexionDB DBScript;  
    public CambiarEscena CambiarEscenaScript;

    public GameObject mercaderiaContainer;
    public GameObject prefabMercaderia;

    private void Start(){
        DBScript.AbrirDB();
        InvocarMercaderia();
    }

    public GameObject panelOpciones;

    private void InvocarMercaderia(){

        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        string sqlQuery = "SELECT * FROM Mercaderia";  
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        while(DBScript.dataReader.Read()){
            
            GameObject prefab = Instantiate(prefabMercaderia);
            prefab.transform.SetParent(mercaderiaContainer.transform, false);

            prefab.transform.GetChild(0).GetComponent<Text>().text = DBScript.dataReader.GetString(0); //Codigo
            prefab.transform.GetChild(1).GetComponent<Text>().text = DBScript.dataReader.GetString(1); //NombreProducto
            prefab.transform.GetChild(2).GetComponent<Text>().text = DBScript.dataReader.GetString(2); //PrecioUnitario
            prefab.transform.GetChild(3).GetComponent<Text>().text = DBScript.dataReader.GetString(3); //Promocion
            prefab.transform.GetChild(4).GetComponent<Text>().text = DBScript.dataReader.GetString(4); //Stock
            prefab.transform.GetChild(5).GetComponent<Text>().text = DBScript.dataReader.GetString(5); //UnidadesVendidas
            prefab.transform.GetChild(6).GetComponent<Text>().text = DBScript.dataReader.GetString(6); //FechaDeCreacion
            prefab.transform.GetChild(7).GetComponent<Text>().text = DBScript.dataReader.GetString(7); //Proveedor
            prefab.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate{AbrirPanelOpciones(prefab);});
        }
    }

    #region Opciones

    public GameObject botonesContainer;
    public GameObject panelEditarArticulo;

    private void AbrirPanelOpciones(GameObject prefab){

        foreach(Transform t in botonesContainer.transform){
            t.GetComponent<Button>().onClick.RemoveAllListeners();
            t.GetComponent<Button>().onClick.AddListener(delegate{MostrarPanelEditarArticulo(t.gameObject.name.ToLower(), prefab);});

            Button botonEliminarArt = botonesContainer.transform.GetChild(botonesContainer.transform.childCount - 2).GetComponent<Button>(); //Busco al anteultimo boton ("Eliminar producto")
            botonEliminarArt.onClick.RemoveAllListeners();
            botonEliminarArt.onClick.AddListener(delegate{AbrirPanelEliminarArticulo(prefab);});
        }

        panelOpciones.SetActive(true);
    }

    public void CerrarPanelOpciones(){

        panelOpciones.SetActive(false);
    }

    private void MostrarPanelEditarArticulo(string opcionNombre, GameObject prefab){

        string codigo = prefab.transform.GetChild(0).GetComponent<Text>().text;
        Text encabezadoTxt = panelEditarArticulo.transform.GetChild(1).GetComponent<Text>();
        Text tituloTxt = panelEditarArticulo.transform.GetChild(2).GetComponent<Text>();
        InputField valorAEditarIF = tituloTxt.transform.GetChild(0).GetComponent<InputField>();
        Button confirmarEdicionBtn = panelEditarArticulo.transform.GetChild(3).GetComponent<Button>();

        CerrarPanelOpciones();

        panelEditarArticulo.transform.GetChild(0).GetComponent<Text>().text = "EDITAR " + opcionNombre.ToUpper(); //Titulo   

        switch(opcionNombre){ //Encabezado y Titulo del IF
            case "promocion":
            encabezadoTxt.text = "Escribir debajo la nueva " + opcionNombre + " para el producto: " + prefab.transform.GetChild(1).GetComponent<Text>().text;
            tituloTxt.text = "NUEVA " + opcionNombre.ToUpper();
            break;

            case "unidades vendidas":
            encabezadoTxt.text = "Escribir debajo las nuevas " + opcionNombre + " para el producto: " + prefab.transform.GetChild(1).GetComponent<Text>().text;
            tituloTxt.text = "NUEVAS " + opcionNombre.ToUpper();
            break;

            case "fecha de creacion":
            encabezadoTxt.text = "Escribir debajo la nueva fecha de creación para el producto: " + prefab.transform.GetChild(1).GetComponent<Text>().text;    
            tituloTxt.text = "NUEVA FECHA"; 
            break;

            case "producto":
            encabezadoTxt.text = encabezadoTxt.text = "Escribir debajo el nuevo nombre del producto: " + prefab.transform.GetChild(1).GetComponent<Text>().text;    
            tituloTxt.text = "NUEVO NOMBRE";
            break;

            default:
            encabezadoTxt.text = "Escribir debajo el nuevo " + opcionNombre + " para el producto: " + prefab.transform.GetChild(1).GetComponent<Text>().text;     
            tituloTxt.text = "NUEVO " + opcionNombre.ToUpper();
            break;

        }

        confirmarEdicionBtn.onClick.RemoveAllListeners();
        confirmarEdicionBtn.onClick.AddListener(delegate{AplicarEdicion(opcionNombre, valorAEditarIF.text, codigo);});

        panelEditarArticulo.SetActive(true);
    }

    public void CerrarPanelEditarArticulo(){
        
        panelEditarArticulo.SetActive(false);

        InputField valorAEditarIF = panelEditarArticulo.transform.GetChild(2).GetChild(0).GetComponent<InputField>();
        valorAEditarIF.text = "";
    }

    private void AplicarEdicion(string columna, string valor, string codigo){

        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        string sqlQuery = String.Format("UPDATE Mercaderia SET \"{0}\" = \"{1}\" WHERE codigo = \"{2}\"", columna, valor, codigo);
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        CambiarEscenaScript.CambiarEscenaA("Mercaderia");       
      
    }

    #region Eliminar Articulo

    public GameObject panelEliminarArticulo;

    public void AbrirPanelEliminarArticulo(GameObject prefab){

        CerrarPanelOpciones();

        string idArticulo = prefab.transform.GetChild(0).GetComponent<Text>().text;
        string nombreArticulo = prefab.transform.GetChild(1).GetComponent<Text>().text;

        Text tituloPanel = panelEliminarArticulo.transform.GetChild(1).GetComponent<Text>();
        tituloPanel.text = "¿Eliminar permanentemente el artículo " + nombreArticulo;

        Button cerrarPanel = panelEliminarArticulo.transform.GetChild(3).GetComponent<Button>();
        cerrarPanel.onClick.RemoveAllListeners();
        cerrarPanel.onClick.AddListener(CerrarPanelEliminarArticulo);

        Button confirmarEliminacionBtn = panelEliminarArticulo.transform.GetChild(2).GetComponent<Button>();
        confirmarEliminacionBtn.onClick.RemoveAllListeners();
        confirmarEliminacionBtn.onClick.AddListener(delegate{ConfirmarEliminacionDeArticulo(idArticulo);});

        panelEliminarArticulo.SetActive(true);

    }

    private void ConfirmarEliminacionDeArticulo(string idArticulo){

        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        string sqlQuery = String.Format("DELETE FROM Mercaderia WHERE codigo = \"{0}\"", idArticulo);
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        CerrarPanelEliminarArticulo();

        CambiarEscenaScript.CambiarEscenaA("Mercaderia");
    }

    private void CerrarPanelEliminarArticulo(){
        panelEliminarArticulo.SetActive(false);
    }

    #endregion Eliminar Articulo

    #endregion Opciones

}
