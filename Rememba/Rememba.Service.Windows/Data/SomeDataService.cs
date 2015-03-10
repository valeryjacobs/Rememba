using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Repositories.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememba.Service.Windows.Data
{
    public class SomeDataService : ISomeDataService
    {
        public async Task<ObservableCollection<ISomeObjectInfo>> GetSubSetOfSomeObjects()
        {
            SomeObjectRepository someObjectRepository = new SomeObjectRepository();

            var someObjects = await someObjectRepository.GetSomeObjects();

            return someObjects;
        }

        public async Task<ISomeObjectDetail> GetSomeObjectDetailWithSetOfSomeOtherObject(string someObjectId)
        {
            SomeObjectRepository someObjectRepository = new SomeObjectRepository();

            var someObject = await someObjectRepository.GetSomeObject();

            return someObject; 
           
        }

        public Task<ISomeOtherObjectDetail> GetSomeOtherObjectDetails(string someOtherObjectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ISomeOtherObjectInfo>> GetSubsetOfSomeOtherObjects()
        {
            throw new NotImplementedException();
        }
    }
}
