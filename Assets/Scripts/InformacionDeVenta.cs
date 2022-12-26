using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InformacionDeVenta : MonoBehaviour {

    //Scripts
    public ConexionDB DBScript;

    private void Start(){
        DBScript.AbrirDB();
        InvocarVendedores();
        InvocarMDPago();
    }

    public Dropdown vendedoresDropdown; 
    public Dropdown mdPagoDropdown;
    public Dropdown cuotasDropdown;

    private void InvocarVendedores(){

        List<string> vendedoresLista = new List<string>().ToList();

        vendedoresLista.Add("Elegir vendedor");
        
        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        // InvocarComidas(String.Format("SELECT * FROM Comidas WHERE Nombre LIKE '%{0}%' OR Equivalencias LIKE '%{0}%' OR Requisitos LIKE '%{0}%' --case-insensitive", busquedaIF.text));        
        string sqlQuery = "SELECT nombre, apellido FROM Empleados WHERE puesto LIKE 'Vendedor@' OR puesto LIKE 'Encargad@'";  
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        while(DBScript.dataReader.Read()){
            string nombreCompleto = DBScript.dataReader.GetString(0) + " " + DBScript.dataReader.GetString(1);
            vendedoresLista.Add(nombreCompleto);
        }

        vendedoresDropdown.AddOptions(vendedoresLista);
    }

    private void InvocarMDPago(){

        List<string> mdPagoLista = new List<string>().ToList();

        mdPagoLista.Add("Elegir medio de pago");
        
        DBScript.dbCommand = DBScript.dbConnection.CreateCommand();
        string sqlQuery = "SELECT medio FROM MediosDePago";  
        DBScript.dbCommand.CommandText = sqlQuery;
        DBScript.dataReader = DBScript.dbCommand.ExecuteReader();

        while(DBScript.dataReader.Read()){
            mdPagoLista.Add(DBScript.dataReader.GetString(0));
        }

        mdPagoDropdown.AddOptions(mdPagoLista);
    }

    public void HabilitarCuotas(){
        cuotasDropdown.interactable = mdPagoDropdown.options[mdPagoDropdown.value].text.Contains("Crédito");

        if(!cuotasDropdown.interactable){ 
            cuotasDropdown.value = 0;
        }
    }

}