using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace TreeViewXamarin
{
    internal class WebApiServices
    {
        #region Fields

        public static string webApiUrl = "https://ej2services.syncfusion.com/production/web-services/api/Orders"; // Your Web Api here

        System.Net.Http.HttpClient client;

        #endregion

        #region Constructor
        public WebApiServices()
        {
            client = new System.Net.Http.HttpClient();
        }
        #endregion

        #region RefreshDataAsync

        /// <summary>
        /// Retrieves data from the web service.
        /// </summary>
        /// <returns>Returns the ObservableCollection.</returns>
        public async System.Threading.Tasks.Task<ObservableCollection<RootTree>> RefreshDataAsync()
        {
            var uri = new Uri(webApiUrl);
            try
            {
                //Sends request to retrieve data from the web service for the specified Uri
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(); //Returns the response as JSON string
                    return JsonConvert.DeserializeObject<ObservableCollection<RootTree>>(content); //Converts JSON string to ObservableCollection
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR {0}", ex.Message);
            }

            return null;
        }
        #endregion
    }
}
