﻿namespace TrackX.Utilities.Static
{
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
        public static List<(string ColumnName, string PropertyName)> GetColumnsCategorias()
        {
            var columnsProperties = new List<(string ColumnName, string PropertyName)>
            {
                ("IDTRA", "title"),
                ("# CONTENEDOR", "new_contenedor"),
                ("# FACTURA", "new_factura"),
                ("BCF", "new_bcf"),
                ("PO", "new_PO"),
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
                ("PROYECCION DE INGRESO", "new_montocostoestimado"),
                ("FACTURAS CF", "new_new_facturacompaia")
            };

            return columnsProperties;
        }
        #endregion

        #region ColumnsWHS
        public static List<(string ColumnName, string PropertyName)> GetColumnsProveedores()
        {
            var columnsProperties = new List<(string ColumnName, string PropertyName)>
            {
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
            };

            return columnsProperties;
        }
        #endregion
    }
}