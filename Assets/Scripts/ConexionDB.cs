using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class ConexionDB : MonoBehaviour {

    #region Conexion

    public string rutaDB;
    public string conexion;
    public IDbConnection dbConnection;
    public IDbCommand dbCommand;
    public IDataReader dataReader;
    public string nombreDB = "dbTienda.db";

    public void AbrirDB(){
        rutaDB = Application.dataPath + "/StreamingAssets/" + nombreDB;
        conexion = "URI=file:" + rutaDB;
        dbConnection = new SqliteConnection(conexion);
        dbConnection.Open();
    }

    public void CerrarDB(){
        dataReader.Close();
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
    #endregion Conexion

}
