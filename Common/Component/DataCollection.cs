namespace Common
{
    using System.Collections.Generic;

    public class DataCollection
    {
        public List<DataList> Rows { get; private set; }

        public DataCollection()
        {
            this.Rows = new List<DataList>();
        }

        public int Count
        {
            get
            {
                return Rows.Count;
            }
        }

        public void Add(DataList dataList)
        {
            if (dataList != null)
            {
                Rows.Add(dataList);
            }
        }

    }
}
