using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MostrarMercaderia : MonoBehaviour {

    //Scripts
    public ConexionDB DBScript;  

    public GameObject mercaderiaContainer;
    public GameObject prefabMercaderia;

    private void Start(){
        DBScript.AbrirDB();
        InvocarMercaderia();
    }


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
            prefab.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(delegate{FuncionRandom(prefab);});
        }
    }

    private void FuncionRandom(GameObject prefab){
        Debug.Log(prefab.transform.GetChild(0).gameObject.name);
    }

}
