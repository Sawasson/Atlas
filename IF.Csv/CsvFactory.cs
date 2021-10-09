using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvFramework
{


    public static class CsvFactory
    {
        static CsvModelGenericDictionary csvModels = new CsvModelGenericDictionary();

        //TODO:away static
        static List<CsvFilterItem> navigationFilters = new List<CsvFilterItem>();


        public static void Register<T>(Action<CsvColumnBuilder<T>> builderAction, bool skipheader, char seperator, string[] lines) where T : class, new()
        {
            var name = typeof(T).Name;

            if (csvModels.IsExist(name)) csvModels.Remove(name);

            CsvColumnBuilder<T> builder = new CsvColumnBuilder<T>();
            builderAction(builder);
            CsvModel<T> csvModel = new CsvModel<T>();
            csvModel.Lines = lines;
            csvModel.Builder = builder;
            csvModel.Seperator = seperator;
            csvModel.SkipHeader = skipheader;
            csvModels.Add(name, csvModel);


        }




        //TODO:Mesaure Performance and 
        public static async Task<List<T>> Parse<T>(IPaseringAsync<T> pasering = null) where T : class, new()
        {

            CsvModel<T> csvModel = csvModels.GetValue<T>(typeof(T).Name);

            var headers = csvModel.Lines.First().Split(csvModel.Seperator);


            List<T> list = new List<T>();

            if (csvModel.SkipHeader)
            {
                csvModel.Lines = csvModel.Lines.Skip(1).ToArray();
            }

            int index = 0;

            if (navigationFilters.Any())
            {
                var currentFilter = navigationFilters.SingleOrDefault(f => f.CsvName == typeof(T).Name);
                if (currentFilter != null)
                {
                    index = csvModel.Builder.Columns.SingleOrDefault(c => c.PropertyName == currentFilter.Name).Index.Value;
                }
            }



            foreach (var line in csvModel.Lines)
            {

                if (String.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(csvModel.Seperator);

                if (navigationFilters.Any())
                {
                    var currentFilter = navigationFilters.SingleOrDefault(f => f.CsvName == typeof(T).Name);
                    if (currentFilter != null)
                    {
                        if (values[index] != currentFilter.Value) continue;
                    }
                }

                var item = new T();

                foreach (var column in csvModel.Builder.Columns)
                {
                    try
                    {
                        if (column.Index.HasValue)
                        {
                            string value = values[column.Index.Value];

                            if (column.Formatter != null)
                            {
                                value = column.Formatter(value);
                            }
                            item.GetType().GetProperty(column.PropertyName).SetValue(item, Convert.ChangeType(value, column.Type, CultureInfo.InvariantCulture));
                        }
                        else if (!String.IsNullOrWhiteSpace(column.ColumnName))
                        {

                            try
                            {
                                var columnIndex = Array.IndexOf(headers, column.ColumnName);

                                string value = values[columnIndex];

                                if (column.Formatter != null)
                                {
                                    value = column.Formatter(value);
                                }
                                item.GetType().GetProperty(column.PropertyName).SetValue(item, Convert.ChangeType(value, column.Type, CultureInfo.InvariantCulture));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            var columnIndex = Array.IndexOf(headers, column.ColumnName);

                            string value = values[columnIndex];

                            if (column.Formatter != null)
                            {
                                value = column.Formatter(value);
                            }
                            item.GetType().GetProperty(column.PropertyName).SetValue(item, Convert.ChangeType(value, column.Type, CultureInfo.InvariantCulture));
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw;
                        string path = @"c:\temp\error_csv.txt";

                        using (StreamWriter w = File.AppendText(path))
                        {
                            w.WriteLine(ex.GetBaseException().Message);
                        };
                    }
                }


                foreach (var navigation in csvModel.Builder.Navigations)
                {
                    string navigationValue = values[csvModel.Builder.Columns.Where(c => c.IsKey).SingleOrDefault().Index.Value];

                    var currentFilter = navigationFilters.SingleOrDefault(f => f.CsvName == navigation.Type.Name);

                    if (currentFilter == null)
                    {
                        navigationFilters.Add(new CsvFilterItem { CsvName = navigation.Type.Name, Name = navigation.NavigationName, Value = navigationValue });
                    }
                    else
                    {
                        currentFilter.Value = navigationValue;
                    }

                    var @object = typeof(CsvFactory)
                          .GetMethod("Parse")
                          .MakeGenericMethod(navigation.Type)
                          .Invoke(null, null);

                    item.GetType().GetProperty(navigation.Name).SetValue(item, @object);

                }

                if (pasering != null)
                {
                    var result = await pasering.ParsingAsync(item);

                    if (!result.SkipThis)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    list.Add(item);
                }
            }

            return list;
        }
    }
}