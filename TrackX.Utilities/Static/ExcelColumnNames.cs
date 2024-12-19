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
            ("# CONTENEDOR 2", "new_contenedor2"),
            ("# FACTURA", "new_factura"),
            ("# FACTURA 2", "new_factura2"),
            ("BCF", "new_bcf"),
            ("PO", "new_po"),
            ("PO 2", "new_po2"),
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
            ("MARCHAMO 2", "new_seal2"),
            ("CANTIDAD DE EQUIPO", "new_cantequipo"),
            ("TAMAÑO DE EQUPO", "new_tamaoequipo"),
            ("CANTIDAD DE BULTOS", "new_contidadbultos"),
            ("CANTIDAD DE BULTOS 2", "new_contidadbultos2"),
            ("SHIPPER", "_new_shipper_value"),
            ("COMMODITY", "new_commodity"),
            ("COMMODITY 2", "new_commodity2"),
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
            ("# RECIBO", "new_numerorecibo"),
            ("NOMBRE PEDIMENTADOR", "new_nombrepedimentador"),
            ("ESTADO", "new_preestado2"),
            ("CLIENTE", "_customerid_value"),
            ("EJECUTIVO", "new_ejecutivocomercial"),
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
            ("TIPO AFORO", "new_tipoaforo"),
            ("# DUA ANTICIPADO", "new_duaanticipados"),
            ("# DUA NACIONAL", "new_duanacional"),
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
            ("FACTURA COMERCIAL", "new_facturacomercial"),
            ("LISTA DE EMPAQUE", "new_listadeempaque"),
            ("DRAFT BL", "new_draftbl"),
            ("BL ORIGINAL", "new_bloriginal"),
            ("CARTA TRAZABILIDAD", "new_cartatrazabilidad"),
            ("CARTA DE DESGLOSE DE CARGOS", "new_cartadesglosecargos"),
            ("EXONERACIONES", "new_exoneracion"),
            ("CERTIFICADO DE ORIGEN", "new_certificadoorigen"),
            ("CERTIDICADO DE RE-EXPORTACION", "new_certificadoreexportacion"),
            ("PERMISOS", "new_permisos"),
            ("BORRADOR DE IMPUESTOS", "new_borradordeimpuestos"),
            ("DOCUMENTO DE NACIONALIZACION", "new_documentodenacionalizacion"),
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
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsCotizaciones
    public static List<(string ColumnName, string PropertyName)> GetColumnsCotizacion()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("QUO", "Quo"),
            ("Cotizacion", "Cotizacion"),
            ("CLIENTE", "NombreCliente"),
            ("FECHA DE REGISTRO", "FechaCreacionAuditoria"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsPanama
    public static List<(string ColumnName, string PropertyName)> GetColumnsPanama()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("IDTRA", "title"),
            ("ESTADO", "new_preestado2"),
            ("CLIENTE", "_customerid_value"),
            ("CANTIDAD DE EQUIPO", "new_cantequipo"),
            ("TAMAÑO DE EQUIPO", "new_tamaoequipo"),
            ("# CONTENEDOR", "new_contenedor"),
            ("CANTIDAD DE BULTOS", "new_contidadbultos"),
            ("PESO", "new_peso"),
            ("PO", "new_po"),
            ("# FACTURA", "new_factura"),
            ("COMMODITY", "new_commodity"),
            ("BCF", "new_bcf"),
            ("POL", "new_pol"),
            ("POE", "new_poe"),
            ("ETA", "new_eta"),
            ("ULTIMO DIA LIBRE CONTENEDOR","new_ultimodialibrecontenedor"),
            ("FECHA SALIDA PTY","new_fechasalidapty"),
            ("FECHA LLEGADA CRC","new_fechallegadacrc"),
            ("CERTIFICADO ORIGEN", "new_certificadoorigen"),
            ("CERTIFICADO REEXPORTACIÓN", "new_certificadoreexportacion"),
            ("PAGO NAVIERA REALIZADO","new_pagonavierarealizado"),
            ("ENTREGA BL ORIGINAL", "new_entregablo"),
            ("ENTREGA CARTA DE TRAZABILIDAD", "new_entregacargatrazabilidad"),
            ("FECHA BL IMPRESO", "new_fechablimpreso"),
            ("ENTREGA DE TRADUCCIÓN", "new_fechatraduccion"),
            ("FECHA DMC ENTRADA", "new_fechadmcentrada"),
            ("FECHA TI", "new_fechati"),
            ("FECHA CARGA HUB PANAMA", "new_fechacargahubpanama"),
            ("FECHA DMC SALIDA", "new_fechadmcsalida"),
            ("FECHA REALIZAR CERT. REEXPORTACION","new_fecharealizarcertreexportacion"),
            ("LIBERACIÓN DOCUMENTAL", "new_fechaliberaciondocumental"),
            ("LIBERACIÓN FINANCIERA", "new_fechaliberacionfinanciera"),
            ("FACTURA COMERCIAL", "new_facturacomercial"),
            ("LISTA DE EMPAQUE", "new_listadeempaque"),
            ("DRAFT BL", "new_draftbl"),
            ("BL ORIGINAL", "new_bloriginal"),
            ("CARTA TRAZABILIDAD", "new_cartatrazabilidad"),
            ("CARTA DE DESGLOSE DE CARGOS", "new_cartadesglosecargos"),
            ("EXONERACIONES", "new_exoneracion"),
            ("BORRADOR CERTIFICADO ORIGEN", "new_borradordecertificadodeorigen"),
            ("CERTIFICADO DE ORIGEN", "new_certificadoorigen"),
            ("CERTIFICADO DE RE-EXPORTACION", "new_certificadoreexportacion"),
            ("CARTA PORTE", "new_cartaporte"),
            ("MANIFIESTO", "new_manifiestoenlace"),
            ("DUCAT", "new_ducatenlace"),
            ("BL NAVIERA / SWB", "new_blnavieraswb"),
            ("CARTA LIBERACION NAVIERA", "new_cartaliberacionnaviera"),
            ("# DMC ENTRADA", "new_dmcentrada"),
            ("DMC ENTRADA", "new_dmcentradaenlace"),
            ("# IT", "new_ti"),
            ("TI", "new_tienlace"),
            ("# DMC SALIDA", "new_dmcsalida"),
            ("DMC SALIDA", "new_dmcsalidaenlace"),
            ("FECHA STATUS CLIENTE", "new_fechastatus"),
            ("STATUS CLIENTE", "new_statuscliente"),
        };

        return columnsProperties;
    }
    #endregion

    #region ColumnsPanamaReporte
    public static List<(string ColumnName, string PropertyName)> GetColumnsPanamaReporte()
    {
        var columnsProperties = new List<(string ColumnName, string PropertyName)>
        {
            ("IDTRA", "title"),
            ("CLIENTE", "_customerid_value"),
            ("POE", "new_poe"),
            ("ETA", "new_eta"),
            ("# CONTENEDOR", "new_contenedor"),
            ("TAMAÑO DE EQUPO", "new_tamaoequipo"),
            ("PESO", "new_peso"),
            ("CANTIDAD DE BULTOS", "new_contidadbultos"),
            ("STAUS CLIENTE", "new_statuscliente"),
            ("FECHA STATUS CLIENTE","new_fechastatus"),
            ("COMENTARIOS OVERNIGHT","new_comentariosovernight"),
            ("ACTUALIZACION OVERNIGHT","new_actualizacionovernight"),
            ("PO", "new_po"),
            ("COMMODITY", "new_commodity"),
            ("APLICA C.O.", "new_aplicacertificadodeorigen"),
        };

        return columnsProperties;
    }
    #endregion
}