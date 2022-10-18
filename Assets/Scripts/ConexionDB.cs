using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class ConexionDB : MonoBehaviour {

    private void Start(){
        AbrirDB();
        VerMercaderia();
    }

    #region ConexionDB

    string rutaDB;
    string conexion;
    IDbConnection dbConnection;
    IDbCommand dbCommand;
    IDataReader dataReader;
    string nombreDB = "dbTienda.db";

    private void AbrirDB(){
        rutaDB = Application.dataPath + "/StreamingAssets/" + nombreDB;
        conexion = "URI=file:" + rutaDB;
        dbConnection = new SqliteConnection(conexion);
        dbConnection.Open();
    }

    private void CerrarDB(){
        dataReader.Close();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
    #endregion ConexionDB
    

    void VerMercaderia(){
        dbCommand = dbConnection.CreateCommand();
        string sqlQuery = "SELECT producto FROM Mercaderia WHERE codigo = 2";
        dbCommand.CommandText = sqlQuery;
        dataReader = dbCommand.ExecuteReader();

        while(dataReader.Read()){
            Debug.Log(dataReader.GetString(0));
        }   
    }

}
