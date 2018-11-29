using Common.PassPort;

namespace Common
{
    using System;
    using System.Web.UI;

    public abstract class PageBase : Page
    {

        protected override void OnUnload(EventArgs e)
        {
            DataConnection.ClearConnection();
            base.OnUnload(e);
            
        }
    }
}
