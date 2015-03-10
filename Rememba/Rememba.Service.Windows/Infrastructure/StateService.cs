using Rememba.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Rememba.Service.Windows.Infrastructure
{
    public class StateService : IStateService
    {
        ApplicationDataContainer localSettings = null;
        public string Parameter { get; set; }
        public string ViewName { get; set; }

        //public StateService()
        //{
        //    localSettings = ApplicationData.Current.LocalSettings;

        //}

        public void AddState(string viewName, string parameter)
        {
            this.ViewName = viewName;
            this.Parameter = parameter;
        }

        public void SaveState()
        {
            localSettings.Values["viewName"] = ViewName;
            localSettings.Values["parameter"] = Parameter;
        }

        public void LoadState()
        {
            ViewName = localSettings.Values["viewName"].ToString();
            Parameter = localSettings.Values["parameter"].ToString();
        }
    }
}
