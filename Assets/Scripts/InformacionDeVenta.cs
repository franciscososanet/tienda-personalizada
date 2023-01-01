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

    #region InformacionVenta

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

    #endregion InformacionVenta

    #region ConfirmarVenta

    public Text unidadesTxt;
    public Text subtotalTxt;
    public Text descuentoTxt;
    public Text totalTxt;
    public InputField observacionesIF;

    public void ConfirmarVenta(){

        string unidades = unidadesTxt.text;
        string subtotal = subtotalTxt.text;
        string descuento = descuentoTxt.text;
        string total = totalTxt.text;
        string vendedor = vendedoresDropdown.options[vendedoresDropdown.value].text;
        string medioDePago = mdPagoDropdown.options[mdPagoDropdown.value].text;
        string cuotas = cuotasDropdown.options[cuotasDropdown.value].text;
        string observaciones = observacionesIF.text;
        string fecha = System.DateTime.Now.ToString("dd/MM/yyyy - hh:mm:ss tt");
        //string nombreDeCaja = ;


        //OBTENER TODOS LOS FICHADOS, ENLISTARLOS (O VER LA MANERA DE HACERLO), Y PASARLOS A LA CAJA ACTUAL ETC. => REVISAR COMO HICE EN EL PROYECTO DE TIENDA PASADO
        //CONEXION A DB: EN LA CAJA ACTUAL, PONER TODOS ESTOS DATOS, RESTARLOS DE LA DB DE MERCADERIA (AL STOCK), SUMARLO A LA VENTA HISTORICA DEL VENDEDOR...

        //SI TODO SALIO CORRECTAMENTE, RECARGAR LA ESCENA ACTUAL (O REINICIAR TODOS LOS VALORES, VER QUE ES MAS FACIL)
        //SI ALGO FALLO, CREAR UN MENSAJE DE ALERTA/ERROR
        
    }

    #endregion ConfirmarVenta


    

}