using Rememba.Contracts.Models;
using Rememba.Contracts.Services;
using Rememba.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Rememba.Repositories.Windows
{
    public class SomeObjectRepository 
    {
        private string baseUri = "http://localhost:xxx/AppRefService.svc/";
        private const string SomeObjectDataFileName = "data.xml";

        //gets from the service all the tile infos

        public async Task<ISomeObjectDetail> GetSomeObject()
        {
            return  new SomeObjectDetail()
            {
                SomeDetailProp = "Yess!!!",
                SomeIntProp = 2,
                SomeStringProp = "Dit is het echte werk!!!"
            };
        }
        public async Task<ObservableCollection<ISomeObjectInfo>> GetSomeObjects()
        {
            ObservableCollection<ISomeObjectInfo> setOfSomeObject = new ObservableCollection<ISomeObjectInfo>();
            string retrievedXml = string.Empty;

            try
            {
                //check cache first
                StorageFolder localFolder =
                    ApplicationData.Current.LocalFolder;
                if (await StorageHelper.DoesFileExistAsync
                    (SomeObjectDataFileName, localFolder))
                {
                    //use cached version
                    StorageFile file =
                        await localFolder.GetFileAsync(SomeObjectDataFileName);
                    retrievedXml = await FileIO.ReadTextAsync(file);
                }
                else //download and store now
                {
                    HttpClientHandler httpClientHandler = new HttpClientHandler();
                    //use this handler for example to pass credentials

                    HttpClient client = new HttpClient(httpClientHandler);
                    HttpResponseMessage response = await client.GetAsync(baseUri + "URI TEMPLATE");
                    if (response != null)
                        retrievedXml = await response.Content.ReadAsStringAsync();

                    //store the response now
                    StorageFile storageFile = await localFolder.CreateFileAsync(SomeObjectDataFileName);
                    await FileIO.WriteTextAsync(storageFile, retrievedXml);
                }

                //read the response into XDocument
                var results = from t in XDocument.Parse(retrievedXml).Descendants("someobject")
                              select t;

                //Now we can parse the XML
                foreach (var continentElement in results)
                {
                    ISomeObjectInfo someObjectInfo = new SomeObjectInfo()
                    {

                        SomeIntProp = 0 ,
                        SomeStringProp = "getfromdeserialization"
                    };

                    List<ISomeOtherObjectInfo> setOfOtherObject = new List<ISomeOtherObjectInfo>();

                    foreach (var travelElement in continentElement.Descendants("Travel"))
                    {
                        ISomeOtherObjectInfo someOtherObjectInfo = new SomeOtherObjectInfo()
                        {
                            SomeIntProp =0 ,
                            SomeStringProp = "getfromdeserialization"
                        };
                        setOfOtherObject.Add(someOtherObjectInfo);
                    }

                    someObjectInfo.SetOfOtherObject= setOfOtherObject;
                    setOfSomeObject.Add(someObjectInfo);
                }

                return setOfSomeObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ISomeObjectDetail> GetSomeOtherData(string continentId)
        {

            try
            {
                //check cache first
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                //use cached version

                //download and store now

                //store the response now

                //read the response into XDocument


                //Now we can parse the XML

                return  new SomeObjectDetail();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
