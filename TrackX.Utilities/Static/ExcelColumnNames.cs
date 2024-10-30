namespace TrackX.Utilities.Static;

public class ExcelColumnNames
{
    public static List<TableColumns> GetColumns(IEnumerable<(string ColumnName, string PropertyName)> columnsProperties)
    {
        var columns = new List<TableColumns>();

        foreach (var (ColumnName, PropertyName) in columnsProperties)
        {
            var column = new TableColumns()
            {
                Label = ColumnName,
                PropertyName = PropertyName
            };

            columns.Add(column);
        }

        return columns;
    }

    #region ColumnsTramitesActivos
    public static List<(string ColumnName, string PropertyName)> GetColumnsTramitesActivos()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("IDTRA", "title"),
            ("# CONTENEDOR", "new_contenedor"),
            ("# FACTURA", "new_factura"),
            ("BCF", "new_bcf"),
            ("PO", "new_po"),
            ("ORIGEN", "new_origen"),
            ("POL", "new_pol"),
            ("ETD", "new_etd1"),
            ("DESTINO", "new_destino"),
            ("POE", "new_poe"),
            ("ETA", "new_eta"),
            ("ESTADO", "new_preestado2"),
            ("ESTADO CLIENTE", "new_statuscliente"),
            ("FECHA MODIFICACION", "modifiedon"),
            ("DIAS DE TRANSITO", "new_diasdetransito"),
            ("CONFIRMACION DE ZARPE", "new_confirmacinzarpe"),
            ("NOMBRE DEL BUQUE", "new_barcodesalida"),
            ("NUMERO DE VIAJE", "new_viajedesalida"),
            ("INCOTERM", "new_incoterm"),
            ("TRANSPORTE", "new_transporte"),
            ("MARCHAMO", "new_seal"),
            ("CANTIDAD DE EQUIPO", "new_cantequipo"),
            ("TAMAÑO DE EQUPO", "new_tamaoequipo"),
            ("CANTIDAD DE BULTOS", "new_contidadbultos"),
            ("SHIPPER", "_new_shipper_value"),
            ("COMMODITY", "new_commodity"),
            ("TARIFA","new_ofertatarifaid"),
            ("PROYECCION DE INGRESO", "new_proyecciondeingreso"),
            ("FACTURAS CF", "new_new_facturacompaia")
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsWHS
    public static List<(string ColumnName, string PropertyName)> GetColumnsWHS()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("ID INTERNO", "Idtra"),
            ("# WHS", "NumeroWHS"),
            ("CLIENTE", "NombreCliente"),
            ("TIPO REGISTRO", "TipoRegistro"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("PO","PO"),
            ("STATUS", "StatusWHS"),
            ("POL","POL"),
            ("POD","POD"),
            ("CANT. BULTOS", "CantidadBultos"),
            ("TIPO BULTOS","TipoBultos"),
            ("VINCULADO","VinculacionOtroRegistro"),
            ("WHS RECEIPT","WHSReceipt"),
            ("DOCUMENTACION REGISTRO","Documentoregistro"),
            ("IMAGEN1","Imagen1"),
            ("IMAGEN2","Imagen2"),
            ("IMAGEN3","Imagen3"),
            ("IMAGEN4","Imagen4"),
            ("IMAGEN5","Imagen5"),
            ("DETALLE","Detalle"),
            ("ESTADO", "EstadoWHS")
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsUsuarios
    public static List<(string ColumnName, string PropertyName)> GetColumnsUsuarios()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("IMAGEN","Imagen"),
            ("NOMBRE", "Nombre"),
            ("APELLIDO", "Apellido"),
            ("CORREO", "Correo"),
            ("CLIENTE","Cliente"),
            ("ROL","Rol"),
            ("EMPRESA","NombreEmpresa"),
            ("TELEFONO","Telefono"),
            ("PAIS","Pais"),
            ("DIRECCION","Direccion"),
            ("FECHA DE CREACION", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoUsuario"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsNoticias
    public static List<(string ColumnName, string PropertyName)> GetColumnsNoticias()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("TITULO", "Titulo"),
            ("SUBTITULO", "Subtitulo"),
            ("CONTENIDO", "Contenido"),
            ("IMAGEN","Imagen"),
            ("FECHA DE CREACION", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoNoticia"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsItinerarios
    public static List<(string ColumnName, string PropertyName)> GetColumnsItinerarios()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("ORIGEN", "Origen"),
            ("POL", "POL"),
            ("DESTINO", "Destino"),
            ("POD", "POD"),
            ("CLOSING", "Closing"),
            ("ETD", "Closing"),
            ("ETA", "Closing"),
            ("CARRIER","Carrier"),
            ("VESSEL","Vessel"),
            ("VOYAGE","Voyage"),
            ("TRANSPORTE","Transporte"),
            ("MODALIDAD","Modalidad"),
            ("FECHA DE CREACION", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoItinerario"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsFinance
    public static List<(string ColumnName, string PropertyName)> GetColumnsFinance()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("CLIENTE", "NombreCliente"),
            ("ESTADO DE CUENTA", "EstadoCuenta"),
            ("FECHA DE CREACION", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoFinance"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsExoneraciones
    public static List<(string ColumnName, string PropertyName)> GetColumnsExoneraciones()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("ID INTERNO", "Idtra"),
            ("CLIENTE", "NombreCliente"),
            ("TIPO EXONERACION", "TipoExoneracion"),
            ("STATUS", "StatusExoneracion"),
            ("PRODUCTO","Producto"),
            ("CATEGORIA","Categoria"),
            ("CLASIFICACION ARANCELARIA","ClasificacionArancelaria"),
            ("# SOLICITUD", "NumeroSolicitud"),
            ("SOLICITUD","Solicitud"),
            ("# AUTORIZACION","NumeroAutorizacion"),
            ("AUTORIZACION","Autorizacion"),
            ("DESDE","Desde"),
            ("HASTA","Hasta"),
            ("DESCRIPCION","Descripcion"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoExoneracion")
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsEmpleos
    public static List<(string ColumnName, string PropertyName)> GetColumnsEmpleos()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("TITULO", "Titulo"),
            ("PUESTO", "Puesto"),
            ("DESCRIPCION", "Descripcion"),
            ("IMAGEN", "Imagen"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoEmpleo")
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsLogs
    public static List<(string ColumnName, string PropertyName)> GetColumnsLogs()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("USUARIO", "Usuario"),
            ("MODULO", "Modulo"),
            ("ACCION", "TipoMetodo"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoLogs"),
            ("PARAMETROS", "Parametros"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsOrigen
    public static List<(string ColumnName, string PropertyName)> GetColumnsOrigen()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("PAIS", "Nombre"),
            ("IMAGEN", "Imagen"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoOrigen"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsDestino
    public static List<(string ColumnName, string PropertyName)> GetColumnsDestino()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("PAIS", "Nombre"),
            ("IMAGEN", "Imagen"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoOrigen"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsPol
    public static List<(string ColumnName, string PropertyName)> GetColumnsPol()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("POL", "Nombre"),
            ("WHS","EstadoWHS"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoPol"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsPod
    public static List<(string ColumnName, string PropertyName)> GetColumnsPod()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("POD", "Nombre"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoPod"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsControlInventario
    public static List<(string ColumnName, string PropertyName)> GetColumnsControlInventario()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("CLIENTE", "NombreCliente"),
            ("CONTROL INVENTARIO", "ControlInventario"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoControlInventario"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsMultimedia
    public static List<(string ColumnName, string PropertyName)> GetColumnsMultimedia()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("ID", "Id"),
            ("NOMBRE", "Nombre"),
            ("MULTIMEDIA", "Multimedia"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoMultimedia"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsTransporteInternacional
    public static List<(string ColumnName, string PropertyName)> GetColumnsTrasporteInternacional()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("IDTRA", "title"),
            ("ESTADO", "new_preestado2"),
            ("CLIENTE", "_customerid_value"),
            ("CLIENTE", "new_ejecutivocomercial"),
            ("# CONTENEDOR", "new_contenedor"),
            ("# FACTURA", "new_factura"),
            ("COMMODITY", "new_commodity"),
            ("BCF", "new_bcf"),
            ("PO", "new_po"),
            ("POL", "new_pol"),
            ("POE", "new_poe"),
            ("ETA", "new_eta"),
            ("CONFIRMACION DE ZARPE", "new_confirmacinzarpe"),
            ("CANTIDAD DE EQUIPO", "new_cantequipo"),
            ("TAMAÑO DE EQUPO", "new_tamaoequipo"),
            ("CANTIDAD DE BULTOS", "new_contidadbultos"),
            ("PESO", "new_peso"),
            ("CERTIFICADO ORIGEN", "new_certificadoorigen"),
            ("CERTIFICADO REEXPORTACIÓN", "new_certificadoreexportacion"),
            ("EXONERACIÓN", "new_exoneracion"),
            ("ENTREGA BL ORIGINAL", "new_entregablo"),
            ("ENTREGA CARTA DE TRAZABILIDAD", "new_entregacargatrazabilidad"),
            ("FECHA BL IMPRESO", "new_fechablimpreso"),
            ("FECHA BL DIGITADO TICA", "new_fechabldigitadotica"),
            ("ENTREGA DE TRADUCCIÓN", "new_fechatraduccion"),
            ("LIBERACIÓN DOCUMENTAL", "new_fechaliberaciondocumental"),
            ("LIBERACIÓN FINANCIERA", "new_fechaliberacionfinanciera"),
            ("COMENTARIO", "new_observacionesgenerales"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsBcf
    public static List<(string ColumnName, string PropertyName)> GetColumnsBcf()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("IDTRA", "Idtra"),
            ("BCF", "Bcf"),
            ("CLIENTE", "NombreCliente"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
            ("ESTADO", "EstadoControlInventario"),
        };

        return columnsProperties;
    }
    #endregion
}