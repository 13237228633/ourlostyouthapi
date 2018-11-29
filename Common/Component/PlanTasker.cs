

namespace Common
{
    using System;
    using System.Collections;
    using System.Threading;

    public delegate void PlanTaskerDelegate();

    public static class PlanTasker
    {

        #region Property

        private static Timer _expiredTimer;

        private static readonly Hashtable Delegates = Hashtable.Synchronized(new Hashtable(10));

       
      

        #endregion

   

        #region CycleRecover

        public static void CycleRecover(object state)
        {
            if (Delegates.Count <= 0) return;
            foreach (DictionaryEntry taskerDelegate in Delegates)
            {
                ((PlanTaskerDelegate) taskerDelegate.Value).Invoke();
            }
        }

        #endregion

        #region AddDelegate

        public static void AddDelegate(string key, PlanTaskerDelegate taskerDelegate)
        {
            if (!Delegates.ContainsKey(key))
            {
                Delegates.Add(key, taskerDelegate);
            }
        }

        #endregion


        #region Start

        public static void Start(TimeSpan expiredInterval)
        {
            _expiredTimer = new Timer(CycleRecover, null, expiredInterval, expiredInterval);
        }

        #endregion

        #region Dispose

        public static void Dispose()
        {
            _expiredTimer.Dispose();
            Delegates.Clear();
        }
        #endregion




    }


}
