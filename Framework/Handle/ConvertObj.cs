using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Handle
{
    /// <summary>
    /// 对象转换
    /// </summary>
    public class ConvertObj
    {
        public ConvertObj()
        { }

        /// <summary>
        /// 把Form Post过来的表单集合转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T FormToClass<T>(NameValueCollection collection)
        {
            T t = Activator.CreateInstance<T>();
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo _target in properties)
            {
                if (_target.CanWrite)
                    _target.SetValue(t, Convert.ChangeType(collection[_target.Name], _target.PropertyType));
            }
            return t;
        }

        /// <summary>
        /// 将一个对象转换成另外一个对象
        /// </summary>
        /// <typeparam name="T">对象T</typeparam>
        /// <typeparam name="K">对象K</typeparam>
        /// <param name="k">对象实例</param>
        /// <returns></returns>
        public static T ClassToClass<T, K>(K k)
        {
            T t = Activator.CreateInstance<T>();
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo[] properties_kk = k.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo _target in properties)
            {
                if (_target.CanWrite)
                {
                    foreach (PropertyInfo _target_kk in properties_kk)
                    {
                        if (_target.Name == _target_kk.Name && _target.PropertyType == _target_kk.PropertyType)
                        {
                            _target.SetValue(t, _target_kk.GetValue(k, null));
                            break;
                        }
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 将DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToModel<T>(DataTable dt)
        {
            // 定义集合    
            List<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = Activator.CreateInstance<T>();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 将对象转换成IDictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ModelToIDictionary<T>(T model)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "sign" && property.Name != "key" && property.Name != "postUrl" && property.Name != "channelSign")
                {
                    if (property.PropertyType.Name == "List`1") continue; //list集不加
                    string name = property.Name;

                    if (property.GetValue(model) != null)
                    {
                        string value = property.GetValue(model).ToString();
                        string newValue = Utility.FilterString(value);
                        if (newValue != value)
                        {
                            throw new Exception("输入数据含有特殊字符");
                        }
                        dic.Add(name, value);
                    }
                    else
                    {
                        dic.Add(name, "");
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 将对象转换成SortedList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static SortedList<string, string> ModelToSortedList<T>(T model)
        {
            SortedList<string, string> sortedList = new SortedList<string, string>();
            try
            {
                Type type = typeof(T);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (property.Name != "sign" && property.Name != "key" && property.Name != "postUrl" && property.Name != "channelSign")
                    {
                        string name = property.Name;
                        if (property.PropertyType.Name == "List`1")
                        {
                            string value = Utility.ToJson(property.GetValue(model));
                            sortedList.Add(name, value);
                        }
                        else if (property.GetValue(model) != null)
                        {
                            string value = property.GetValue(model).ToString();
                            sortedList.Add(name, value);
                        }
                        else
                        {
                            sortedList.Add(name, "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sortedList;
        }
    }
}
