using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace nomemTools
{
    public static partial class winForm_treeViewPopulation
    {
        /// <summary>
        /// Populate winform treeview with any given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TSource"></param>
        /// <param name="objectName">Initial object name (Optional)</param>
        /// <returns></returns>
        public static TreeNode ObjectAnalizer<T>(T TSource, string objectName = "Object")
        {
            TreeNode treeNodes = new TreeNode(objectName);

            if (TSource is IEnumerable)
            {
                var enumerator = (TSource as IEnumerable).GetEnumerator();

                if (enumerator.MoveNext())
                {
                    if (/*!enumerator.Current.GetType().IsPrimitive 
                        && enumerator.Current.GetType() != typeof(string)
                        && enumerator.Current.GetType() != typeof(DateTime)*/
                        enumerator.Current.GetType().Namespace != "System")
                    {
                        foreach (var item in TSource as IEnumerable)
                        {
                            treeNodes.Nodes.Add(ObjectAnalizer(item, enumerator.Current.GetType().Name));
                        }
                    }
                    else
                    {
                        foreach (var item in TSource as IEnumerable)
                        {
                            treeNodes.Nodes.Add(item.ToString());
                        }
                    }
                }
            }
            else if (TSource.GetType().IsPrimitive)
            {
                treeNodes.Nodes.Add(TSource.ToString());
            }
            else
            {
                PropertyInfo[] propertyInfos = TSource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var item in propertyInfos)
                {
                    if (TSource.GetType().GetProperty(item.Name).GetValue(TSource, null) != null
                        && TSource.GetType().GetProperty(item.Name).GetValue(TSource, null).GetType().Namespace != "System")
                    {
                        treeNodes.Nodes.Add(ObjectAnalizer(TSource.GetType().GetProperty(item.Name).GetValue(TSource, null), item.Name));
                    }
                    else if (TSource.GetType().GetProperty(item.Name).GetValue(TSource, null) != null
                        && TSource.GetType().GetProperty(item.Name).GetValue(TSource, null).GetType().Namespace == "System")
                    {
                        if (TSource.GetType().GetProperty(item.Name).GetValue(TSource, null).GetType().IsArray)
                        {
                            treeNodes.Nodes.Add(ObjectAnalizer(TSource.GetType().GetProperty(item.Name).GetValue(TSource, null), item.Name));
                        }
                        else
                        {
                            treeNodes.Nodes.Add(item.Name, item.Name + ": " + TSource.GetType().GetProperty(item.Name).GetValue(TSource, null).ToString());
                        }
                    }
                }
            }
            return treeNodes;
        }
    }
}
