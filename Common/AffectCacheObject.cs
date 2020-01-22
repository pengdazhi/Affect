using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AffectCacheObject
    {
        /// <summary>
        /// 字典
        /// </summary>
        private static Dictionary<string, object> _dataDic = new Dictionary<string, object>();


        /// <summary>
        /// 定义一个静态变量来保存类的实例
        /// </summary>
        private static AffectCacheObject _session;

        /// <summary>
        /// 定义一个标识确保线程同步
        /// </summary>
        private static readonly object _locker = new object();


        /// <summary>
        /// 单例
        /// </summary>
        /// <returns>返回类型为Session</returns>
        public static AffectCacheObject Instance
        {
            get
            {
                if (_session == null)
                {
                    lock (_locker)
                    {
                        if (_session == null)// 如果类的实例不存在则创建，否则直接返回
                        {
                            _session = new AffectCacheObject();
                        }
                    }
                }
                return _session;
            }
        }

        #region Remove 移除

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            _dataDic.Remove(name);
        }

        /// <summary>
        /// 删除全部成员
        /// </summary>
        public void RemoveAll()
        {
            _dataDic.Clear();
        }
        #endregion

        #region 本类的索引器

        /// <summary>
        /// 本类的索引器
        /// </summary>
        /// <returns>返回Object成员</returns>
        public Object this[string index]
        {
            get
            {
                if (_dataDic.ContainsKey(index))
                {
                    Object obj = (Object)_dataDic[index];
                    return obj;
                }
                return null;
            }
            set
            {
                if (_dataDic.ContainsKey(index))
                {
                    _dataDic.Remove(index);
                }
                _dataDic.Add(index, value);
            }
        }
        #endregion


    }
}
