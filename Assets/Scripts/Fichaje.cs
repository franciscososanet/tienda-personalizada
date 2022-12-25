using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fichaje : MonoBehaviour {

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

    private void Start(){
        ReiniciarValoresFichaje();
        DBScript.AbrirDB();
    }

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

        ReiniciarValoresFichaje();
        ActualizarCalculos();
    }

    #region CalculosLateral

    public GameObject[] valoresCalculosLateral;

    private void ActualizarCalculos(){

        int unidades = 0;
        double subtotal = 0;
        //double descuento = 0;
        double total = 0;

        foreach(Transform t in bodyContainer.transform){
            unidades += Int32.Parse(t.GetChild(3).GetComponent<Text>().text); 
            subtotal += double.Parse(t.GetChild(4).GetComponent<Text>().text); 
            total += double.Parse(t.GetChild(4).GetComponent<Text>().text); 
        }

        valoresCalculosLateral[0].GetComponent<Text>().text = unidades.ToString();
        valoresCalculosLateral[1].GetComponent<Text>().text = subtotal.ToString("0.00");
        valoresCalculosLateral[3].GetComponent<Text>().text = total.ToString("0.00");
    }

    #endregion
}
