using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Log;

namespace Framework.Handle
{
    /// <summary>
    /// 反射帮助类 类的完全限定名=命名空间.类+程序集DLL名称
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 通过反射返回对象
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <returns></returns>
        public static T Instance<T>(string nameSpace, string className, string assemblyName = null)
        {
            T handel = default(T);
            if (!String.IsNullOrEmpty(nameSpace) && !String.IsNullOrEmpty(className) && !String.IsNullOrEmpty(assemblyName))
            {
                handel = ReflectionHelper.CreateInstance<T>(nameSpace, className, assemblyName);
            }
            else if (!String.IsNullOrEmpty(nameSpace) && !String.IsNullOrEmpty(className))
            {
                handel = ReflectionHelper.CreateInstance<T>(nameSpace, className);
            }

            return handel;
        }

        /// <summary>
        ///  创建对象实例 [如果与执行代码同一个程序集.则可以这样调用]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        private static T CreateInstance<T>(string nameSpace, string className)
        {
            try
            {
                string fullName = nameSpace + "." + className; //命名空间.类型名
                var obj = Assembly.GetExecutingAssembly().CreateInstance(fullName, false);
                return (T)obj;//类型转换并返回
            }
            catch
            {
                //发生异常，返回类型的默认值
                return default(T);
            }
        }

        /// <summary>
        /// 创建对象实例 [不同程序集的则要装载调用Load]
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <param name="assemblyName">类型所在程序集名称 dll</param>
        /// <returns></returns>
        private static T CreateInstance<T>(string nameSpace, string className, string assemblyName)
        {
            try
            {
                string path = nameSpace + "." + className + "," + assemblyName;//命名空间.类型名,程序集
                Type t = Type.GetType(path);//加载类型

                //使用区分大小写的搜索，从此程序集中查找指定的类型，然后使用系统激活器创建它的实例。
                //Assembly类中的CreateInstance方法是调用了Activator.CreateInstance方法
                var obj = Assembly.Load(t.Assembly.GetName()).CreateInstance(t.FullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例

                ////使用与指定参数匹配程度最高的构造函数来创建指定类型的实例。
                //var obj = Activator.CreateInstance(t, true);//根据类型创建实例

                return (T)obj;//类型转换并返回
            }
            catch (Exception ex)
            {
                LogService.LogDebug(ex);
                //发生异常，返回类型的默认值
                return default(T);
            }
        }
    }
}
