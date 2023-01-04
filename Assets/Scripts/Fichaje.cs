using System;
using UnityEngine;
using UnityEngine.UI;

public class Fichaje : MonoBehaviour {

    #region Inicializacion

    //Scripts
    public ConexionDB DBScript;

    //UI header
    public InputField codigoIF;
    public Text nombreProductoTxt;
    public Text precioUnitarioTxt;
    public InputField cantidadIF;
    public Text subtotalTxt;
    public Button ficharBtn;

    public GameObject prefabArticuloFichado;
    public GameObject bodyContainer;

    private void Update(){
        ActualizarCalculos();    
    }

    private void Start(){
        ReiniciarValoresFichaje();
        DBScript.AbrirDB();
    }

    #endregion Inicializacion

    #region Fichaje

    private void ReiniciarValoresFichaje(){
        codigoIF.text = "...";
        nombreProductoTxt.text = "...";
        precioUnitarioTxt.text = "0.00";
        cantidadIF.text = "0";
        subtotalTxt.text = "0.00";
        cantidadIF.interactable = false;
        ficharBtn.interactable = false;
    }

    public void BuscarArticulo(){
        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        string sqlQuery = String.Format("SELECT * FROM Mercaderia WHERE codigo = \"{0}\"", codigoIF.text);  
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        if(DBScript.dataReader.Read()){

            string producto = DBScript.dataReader.GetString(1);
            double precioUnitario = double.Parse(DBScript.dataReader.GetString(2));

            nombreProductoTxt.text = producto;
            precioUnitarioTxt.text = precioUnitario.ToString("0.00");

            cantidadIF.interactable = (codigoIF.text.Length > 0 && nombreProductoTxt.text.Length > 0 && nombreProductoTxt.text != null);
            cantidadIF.text = "1";

            CalcularSubtotal();

        }else{
            ReiniciarValoresFichaje();
        }
    }

    public void CalcularSubtotal(){

        if(cantidadIF.text.Length < 1 || cantidadIF.text == null){
            cantidadIF.text = "1";
        }

        double subtotal = double.Parse(precioUnitarioTxt.text) * double.Parse(cantidadIF.text);
        subtotalTxt.text = subtotal.ToString("0.00");

        ficharBtn.interactable = (codigoIF.text.Length > 0 && nombreProductoTxt.text.Length > 0 && cantidadIF.text.Length > 0 && subtotalTxt.text.Length > 0);
    }

    public void FicharArticulo(){

        GameObject prefab = Instantiate(prefabArticuloFichado);
        prefab.transform.SetParent(bodyContainer.gameObject.transform, false);

        string codigoArt = prefab.transform.GetChild(0).gameObject.GetComponent<Text>().text = codigoIF.text;
        string nombreArt = prefab.transform.GetChild(1).gameObject.GetComponent<Text>().text = nombreProductoTxt.text;
        string puArt = prefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = precioUnitarioTxt.text;
        string cantidadArt = prefab.transform.GetChild(3).gameObject.GetComponent<Text>().text = cantidadIF.text;
        string subtotalArt = prefab.transform.GetChild(4).gameObject.GetComponent<Text>().text = subtotalTxt.text;

        prefab.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate{AbrirPanelOpciones(prefab.gameObject);});

        ReiniciarValoresFichaje();
        ActualizarCalculos();
    }

    #endregion Fichaje

    #region PanelOpciones

    public GameObject panelOpcionesArtFichado;

    private void AbrirPanelOpciones(GameObject g){
        if(panelOpcionesArtFichado.activeSelf == false){ 
            panelOpcionesArtFichado.SetActive(true); 

            panelOpcionesArtFichado.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            panelOpcionesArtFichado.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate{CerrarPanel(panelOpcionesArtFichado);});

            panelOpcionesArtFichado.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            panelOpcionesArtFichado.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate{MostrarPanelEditarCantidad(g);});

            panelOpcionesArtFichado.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            panelOpcionesArtFichado.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate{MostrarPanelEditarSubtotal(g.gameObject); });

            panelOpcionesArtFichado.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
            panelOpcionesArtFichado.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate{EliminarArticulo(g.gameObject); });   

        }
    }

    public void CerrarPanel(GameObject panelGO = null){
        if(panelGO.activeSelf){ 
            panelGO.SetActive(false);
        }
    }

    #region Opción: Editar Cantidad`

    public GameObject panelEditarCantidad;
    public Text descripcionEditarCantidadTxt;
    public InputField nuevaCantidadIF;
    public Button editarCantidadBtn;
    public Button cerrarPanelEditarCantidadBtn;

    private void MostrarPanelEditarCantidad(GameObject fichadoGO = null){

        cerrarPanelEditarCantidadBtn.onClick.RemoveAllListeners();
        cerrarPanelEditarCantidadBtn.onClick.AddListener(delegate{CerrarPanel(panelEditarCantidad); });

        string nombreArticulo = "";
        nuevaCantidadIF.text = "0";

        nombreArticulo = fichadoGO.transform.GetChild(1).GetComponent<Text>().text;
        int cantidadActual = Int32.Parse(fichadoGO.transform.GetChild(3).GetComponent<Text>().text);

        descripcionEditarCantidadTxt.text = "La cantidad actual del producto " + nombreArticulo + " es de " + cantidadActual + " unidades.";

        editarCantidadBtn.onClick.RemoveAllListeners();
        editarCantidadBtn.onClick.AddListener(delegate{EditarCantidad(fichadoGO, nuevaCantidadIF.text); });

        CerrarPanel(panelOpcionesArtFichado);
        panelEditarCantidad.SetActive(true);
    }

    private void EditarCantidad(GameObject fichadoGO, string nuevaCantidad){

        fichadoGO.transform.GetChild(3).GetComponent<Text>().text = nuevaCantidad;
        fichadoGO.transform.GetChild(4).GetComponent<Text>().text = (float.Parse(nuevaCantidad) * float.Parse(fichadoGO.transform.GetChild(4).GetComponent<Text>().text)).ToString("0.00"); //Multiplicar subtotal con la nueva cantidad

        CerrarPanel(panelEditarCantidad);
    }

    #endregion Opción: Editar Cantidad

    #region Opción: Editar Subtotal

        public GameObject panelEditarSubtotal;
        public Text descripcionEditarSubtotalTxt;
        public InputField nuevoSubtotalIF;
        public Button editarSubtotalBtn;
        public Button cerrarPanelEditarSubtotalBtn;

        private void MostrarPanelEditarSubtotal(GameObject fichadoGO = null){

            cerrarPanelEditarSubtotalBtn.onClick.RemoveAllListeners();
            cerrarPanelEditarSubtotalBtn.onClick.AddListener(delegate{CerrarPanel(panelEditarSubtotal); });

            string nombreArticulo = "";
            nuevoSubtotalIF.text = "0";

            nombreArticulo = fichadoGO.transform.GetChild(1).GetComponent<Text>().text;
            float subtotalActual = float.Parse(fichadoGO.transform.GetChild(4).GetComponent<Text>().text);

            descripcionEditarSubtotalTxt.text = "El subtotal actual del producto " + nombreArticulo + " es de $" + subtotalActual + ".";

            editarSubtotalBtn.onClick.RemoveAllListeners();
            editarSubtotalBtn.onClick.AddListener(delegate{EditarSubtotal(fichadoGO, nuevoSubtotalIF.text); });

            CerrarPanel(panelOpcionesArtFichado);
            panelEditarSubtotal.SetActive(true);
        }

        private void EditarSubtotal(GameObject fichadoGO, string nuevoSubtotal){

            fichadoGO.transform.GetChild(4).GetComponent<Text>().text = float.Parse(nuevoSubtotal).ToString("0.00"); //Reformateo para poner 2 decimales
            CerrarPanel(panelEditarSubtotal);
        }

    #endregion Opción: Editar Subtotal

    #region Opción: Eliminar Artículo
    private void EliminarArticulo(GameObject g){

        Destroy(g.gameObject);
        CerrarPanel(panelOpcionesArtFichado);
        // ActualizarCalculos();  //Actualmente la funcion esta siendo llamada desde el update: a corregir a futuro.
    }

    #endregion Opción: Eliminar Articulo
    #endregion PanelOpciones

    #region DescuentoGlobal

    public GameObject panelDescuentoGlobal;
    public Text descripcionDescuentoGlobalTxt;
    public InputField descuentoNumeroIF;
    public InputField descuentoPorcentualIF;
    public Button aplicarDescuentoBtn;

    public Text descuentoPesoTxt;
    public Text descuentoPorcentualTxt;

    public void MostrarPanelDescuentoGlobal(){

        descuentoNumeroIF.text = "0";
        descuentoPorcentualIF.text = "0";

        string subtotalActual = valoresCalculosLateral[1].GetComponent<Text>().text;
        descripcionDescuentoGlobalTxt.text = "El subtotal actual de la compra es de $" + subtotalActual;

        panelDescuentoGlobal.SetActive(true);
    }

    public void CalcularDescuento(){

        double subtotalActual = double.Parse(valoresCalculosLateral[1].GetComponent<Text>().text);

        double descuentoPesos = 0;
        double subtotalConDescuentoPesos = 0;
        double descuentoPorcentual = 0;
        double subtotalConDescuentoPorcentual = 0;
        double descuentoTotal = 0;

        descuentoPesos = double.Parse(descuentoNumeroIF.text);
        subtotalConDescuentoPesos = subtotalActual - descuentoPesos;
        descuentoPesoTxt.text = subtotalConDescuentoPesos.ToString("0.00");


        descuentoPorcentual = subtotalConDescuentoPesos - (subtotalConDescuentoPesos * double.Parse(descuentoPorcentualIF.text)) / 100;
        descuentoPorcentualTxt.text = descuentoPorcentual.ToString("0.00");

        subtotalConDescuentoPorcentual = subtotalConDescuentoPesos - descuentoPorcentual;

        descuentoTotal = subtotalActual - descuentoPorcentual;

        aplicarDescuentoBtn.onClick.RemoveAllListeners();
        aplicarDescuentoBtn.onClick.AddListener(delegate{AplicarDescuento(descuentoTotal); });

        Debug.Log("descuento Pesos: " + descuentoPesos);
        Debug.Log("descuento Porcentual: " + descuentoPorcentual);
        Debug.Log("descuento Total: " + descuentoTotal); //Algo de todo esto esta mal. Se est apasando como descuento lo que es el subtotal. En descuento va el descuento restado al subtotal

    }

    private void AplicarDescuento(double descuentoAAplicar){

        Debug.Log(descuentoAAplicar);

        valoresCalculosLateral[2].GetComponent<Text>().text = "0.00"; //Reinicio el descuento, por si se quiere aplicar otro descuento, que no tome en cuenta al anterior.
        valoresCalculosLateral[2].GetComponent<Text>().text = (double.Parse(valoresCalculosLateral[2].GetComponent<Text>().text) + descuentoAAplicar).ToString("0.00");
        CerrarPanel(panelDescuentoGlobal);
    }

    #endregion Descuento Global

    #region CalculosLateral

    public GameObject[] valoresCalculosLateral;

    public void ActualizarCalculos(){

        int unidades = 0;
        double subtotal = 0;

        foreach(Transform t in bodyContainer.transform){
            unidades += Int32.Parse(t.GetChild(3).GetComponent<Text>().text); 
            subtotal += double.Parse(t.GetChild(4).GetComponent<Text>().text); 
        }

        valoresCalculosLateral[0].GetComponent<Text>().text = unidades.ToString();
        valoresCalculosLateral[1].GetComponent<Text>().text = subtotal.ToString("0.00");
        valoresCalculosLateral[3].GetComponent<Text>().text = (subtotal - double.Parse(valoresCalculosLateral[2].GetComponent<Text>().text)).ToString("0.00"); //total = subtotal-descuento
    }

    #endregion CalculosLateral
}
