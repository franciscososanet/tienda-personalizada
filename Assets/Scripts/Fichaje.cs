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

    private void Start(){
        ReiniciarValores();
        DBScript.AbrirDB();
    }

    private void ReiniciarValores(){
        codigoIF.text = null;
        nombreProductoTxt.text = "...";
        precioUnitarioTxt.text = "...";
        cantidadIF.text = "0";
        subtotalTxt.text = "0";
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
            ReiniciarValores();
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

}
