using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Timer
{
    public class TimerInfo
    {
        //定义委托类  
        public delegate void TimerTaskDelegate();

        //当前执行程序名称  
        public string name = "";
        //几秒后再执行任务  
        public int second;
        //定义委托实例  
        private TimerTaskDelegate fun;

        //定义一个线程  
        //线程是不可序列化  
        [NonSerialized]
        private Thread t;

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="name">线程名称</param>  
        /// <param name="second">几秒后再执行任务</param>  
        /// <param name="fun">要执行的任务</param>  
        public TimerInfo(string name, int second, TimerTaskDelegate fun)
        {
            this.name = name;
            this.second = second;
            this.fun = fun;
        }

        /// <summary>  
        /// 启动定时任务  
        /// ApartmentState.Unknown:表示没有设置线程的单元状态
        /// </summary>  
        public void Star()
        {
            t = new Thread(NewThreadStar);
            t.IsBackground = true;
            t.Name = this.name;
            t.Start();
        }

        /// <summary>
        /// MTA:表示Thread将被创建并进入一个多线程单元
        /// </summary>
        public void StarMTA()
        {
            t = new Thread(NewThreadStar);
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.MTA);
            t.Name = this.name;
            t.Start();
        }

        /// <summary>
        /// STA:表示Thread将被创建并进入一个单线程单元
        /// </summary>
        public void StarSTA()
        {
            t = new Thread(NewThreadStar);
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Name = this.name;
            t.Start();
        }

        /// <summary>  
        /// 线程调用的方法  
        /// </summary>  
        private void NewThreadStar()
        {
            //要执行的目标方法  
            fun();

            //当前线程挂起指定时间  
            Thread.Sleep(second * 1000);

            //递归调用实现循环  
            NewThreadStar();
        }

        /// <summary>  
        /// 终止定时任务  
        /// </summary>  
        public void Abort()
        {
            if (t != null && t.IsAlive)
            {
                t.Abort();
            }
        }
    }
}
