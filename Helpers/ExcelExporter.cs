using System.Reflection;
using ClosedXML.Excel;

namespace GrupoTecnofix_Api.Helpers
{
    public static class ExcelExporter
    {
        public static byte[] ExportToExcel<T>(IEnumerable<T> items, string sheetName = "Sheet1")
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(sheetName);

            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var columns = new List<(PropertyInfo? Parent, PropertyInfo Prop)>();

            bool IsSimple(Type t)
                => t.IsPrimitive || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(DateTime?) || t.IsEnum || (Nullable.GetUnderlyingType(t)?.IsEnum ?? false);

            foreach (var p in props)
            {
                if (IsSimple(p.PropertyType))
                {
                    columns.Add((null, p));
                }
                else
                {
                    // flatten one level
                    var subProps = p.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(sp => IsSimple(sp.PropertyType));

                    foreach (var sp in subProps)
                    {
                        columns.Add((p, sp));
                    }
                }
            }

            // write header
            for (int c = 0; c < columns.Count; c++)
            {
                var col = columns[c];
                var header = col.Parent == null ? col.Prop.Name : $"{col.Parent.Name}.{col.Prop.Name}";
                ws.Cell(1, c + 1).Value = header;
                ws.Cell(1, c + 1).Style.Font.Bold = true;
            }

            int row = 2;
            foreach (var item in items)
            {
                for (int c = 0; c < columns.Count; c++)
                {
                    var col = columns[c];
                    object? value = null;
                    try
                    {
                        if (col.Parent == null)
                        {
                            value = col.Prop.GetValue(item);
                        }
                        else
                        {
                            var parentVal = col.Parent.GetValue(item);
                            if (parentVal != null)
                                value = col.Prop.GetValue(parentVal);
                        }
                    }
                    catch { value = null; }

                    // convert common types
                    if (value == null)
                        ws.Cell(row, c + 1).Value = "";
                    else if (value is DateTime dt)
                        ws.Cell(row, c + 1).Value = dt;
                    else if (value is decimal dec)
                        ws.Cell(row, c + 1).Value = dec;
                    else if (value is int i)
                        ws.Cell(row, c + 1).Value = i;
                    else if (value is bool b)
                        ws.Cell(row, c + 1).Value = b;
                    else
                        ws.Cell(row, c + 1).Value = value.ToString();
                }
                row++;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
