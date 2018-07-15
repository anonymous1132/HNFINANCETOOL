using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections.ObjectModel;


namespace CaoJin.HNFinanceTool.Basement
{
  public  class ModelConvertHelper<T> where T : new()
    {
        public static IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
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
                        {
                            //if (value is DateTime)
                            //{
                            //    pi.SetValue(t, ((DateTime)value).ToShortDateString(), null);
                            //}
                            //else { pi.SetValue(t, value, null); }
                            pi.SetValue(t, value, null);
                        }
                      
                    }
                }
                ts.Add(t);
            }
            return ts;
        } 
    

       // **//// <summary>
        /// 转换IList<T>为List<T>      //将IList接口泛型转为List泛型类型
        /// </summary>
        /// <typeparam name="T">指定的集合中泛型的类型</typeparam>
        /// <param name="gbList">需要转换的IList</param>
        /// <returns></returns>
        public static List<T> ConvertIListToList<T>(IList<T> gbList) where T : class   //静态方法，泛型转换，
        {
            if (gbList != null && gbList.Count >= 1)
            {
                List<T> list = new List<T>();
                for (int i = 0; i < gbList.Count; i++)  //将IList中的元素复制到List中
                {
                    T temp = gbList[i] as T;
                    if (temp != null)
                        list.Add(temp);
                }
                return list;
            }
            return null;
        }

        //转换为ObservableCollection
        public static ObservableCollection<T> ConvertToObc(DataTable dt)
        {
            ObservableCollection<T> obc = new ObservableCollection<T>();
            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
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
                        {
                            //if (value is Int32)
                            //{
                            //    pi.SetValue(t, Convert.ToInt32(value), null);
                            //}
                            //else { pi.SetValue(t, value, null); }
                            pi.SetValue(t, value, null);
                        }

                    }
                }
                obc.Add(t);
            }

            return obc;
        }
    
    }
}
