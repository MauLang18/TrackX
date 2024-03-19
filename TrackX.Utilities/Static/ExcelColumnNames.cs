namespace TrackX.Utilities.Static
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
                ("new_statuscliente", "new_statuscliente"),
                ("new_bcf", "new_bcf"),
                ("new_contenedor", "new_contenedor"),
                ("new_po", "new_po")
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