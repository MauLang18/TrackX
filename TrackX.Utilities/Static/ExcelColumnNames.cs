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

        #region ColumnsCategorias
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

        #region ColumnsProveedores
        public static List<(string ColumnName, string PropertyName)> GetColumnsProveedores()
        {
            var columnsProperties = new List<(string ColumnName, string PropertyName)>
            {
                ("NOMBRE", "Nombre"),
                ("EMAIL", "Correo"),
                ("TIPO DE DOCUMENTO", "TipoDocumento"),
                ("N° DE DOCUMENTO","NumeroDocumento"),
                ("DIRECCION", "Direccion"),
                ("TELÉFONO","Telefono"),
                ("FECHA DE CREACIÓN", "FechaCreacionAuditoria"),
                ("ESTADO", "EstadoProveedor")
            };

            return columnsProperties;
        }
        #endregion
    }
}