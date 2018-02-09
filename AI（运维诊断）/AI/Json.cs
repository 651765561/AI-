using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace AI
{
    /// <summary>
    /// Json操作
    /// </summary>
    public static class Json
    {
        public static object ToJson(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject(json);
        }
        /// <summary>
        ///     生成JSON字符串
        /// </summary>
        /// <param name="obj">生成json的对象</param>
        /// <param name="formatjson">是否格式化</param>
        /// <returns></returns>
        public static string ToJson(object obj, bool formatjson)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            var idtc = new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"};

            var serializer = new JsonSerializer();
            serializer.Converters.Add(idtc);
            JsonWriter jw = new JsonTextWriter(sw);

            if (formatjson)
            {
                jw.Formatting = Formatting.Indented;
            }

            serializer.Serialize(jw, obj);

            //JsonConvert.SerializeObject(dt, idtc).ToString();

            return sb.ToString();
        }
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static T ToObject<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
        public static List<T> ToList<T>(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<List<T>>(json);
        }
        public static DataTable ToTable(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<DataTable>(json);
        }
        public static dynamic ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }
      
        /// <summary>
        /// 格式化EASYUI DATAGRID JSON
        /// </summary>
        /// <param name="recordcount">总记录数</param>
        /// <param name="rowsList"></param>
        /// <returns></returns>
        public static string FormatJsonForEasyuiDataGrid(int recordcount, dynamic rowsList)
        {
            return ToJson(new { total = recordcount, rows = rowsList });
        }
    }
}
