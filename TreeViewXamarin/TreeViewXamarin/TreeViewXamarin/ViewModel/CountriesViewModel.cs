using Newtonsoft.Json;
using Syncfusion.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TreeViewXamarin
{
    public class CountriesViewModel : INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<Countries> countriesInfo;
        private ObservableCollection<object> checkedItems;

        WebApiServices webApiServices;
        #endregion

        #region Properties

        public ObservableCollection<Countries> CountriesInfo
        {
            get { return countriesInfo; }
            set { this.countriesInfo = value; this.OnPropertyChanged("CountriesInfo"); }
        }


        public ObservableCollection<object> CheckedItems
        {
            get { return checkedItems; }
            set { this.checkedItems = value; this.OnPropertyChanged("CheckedItems"); }
        }

        public ICommand TreeViewOnDemandCommand { get; set; }

        #endregion

        #region Constructor

        public CountriesViewModel()
        {
            webApiServices = new WebApiServices();
            CountriesInfo = new ObservableCollection<Countries>();
            CheckedItems = new ObservableCollection<object>();
            TreeViewOnDemandCommand = new Command(ExecuteOnDemandLoading, CanExecuteOnDemandLoading);

            GetParentNodes();
        }
        #endregion

        #region Methods

        /// <summary>
        /// CanExecute method is called before expanding and initialization of node. Returns whether the node has child nodes or not.
        /// Based on return value, expander visibility of the node is handled.  
        /// </summary>
        /// <param name="sender">TreeViewNode is passed as default parameter </param>
        /// <returns>Returns true, if the specified node has child items to load on demand and expander icon is displayed for that node, else returns false and icon is not displayed.</returns>
        private bool CanExecuteOnDemandLoading(object sender)
        {
            return ((sender as TreeViewNode).Content as Countries).HasChildNodes;
        }

        /// <summary>
        /// Execute method is called when any item is requested for load-on-demand items.
        /// </summary>
        /// <param name="obj">TreeViewNode is passed as default parameter.</param>
        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;

            // Skip the repeated population of child items when every time the node expands.
            if (node.ChildNodes.Count > 0)
            {
                node.IsExpanded = true;
                return;
            }

            //Animation starts for expander to show progressing of load on demand
            node.ShowExpanderAnimation = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                //Fetching child items to add
                var items = await GetDataAsync();
                await Task.Delay(500);

                // Populating child items for the node in on-demand
                node.PopulateChildNodes(items);
                if (items.Count() > 0)
                    //Expand the node after child items are added.
                    node.IsExpanded = true;

                //Stop the animation after load on demand is executed, if animation not stopped, it remains still after execution of load on demand.
                node.ShowExpanderAnimation = false;
            });
        }

        /// <summary>
        /// Gets data from the web service and return the collection of fetched data.
        /// </summary>
        private async Task<ObservableCollection<Countries>> GetDataAsync()
        {
            var rootItems = await webApiServices.RefreshDataAsync();
            return this.ProcessChildData(rootItems);
        }

        /// <summary>
        /// 
        /// </summary>
        private async void GetParentNodes()
        {
            var rootItems = await webApiServices.RefreshDataAsync();
            CountriesInfo = await ProcessParentNodes(rootItems);
        }

        /// <summary>
        /// Process the fetched data with child items. 
        /// </summary>
        /// <param name="rootData">Data fetched from the web service.</param>
        /// <returns>Returns the processed data for child nodes.</returns>
        private ObservableCollection<Countries> ProcessChildData(ObservableCollection<RootTree> rootData)
        {
            var details = new ObservableCollection<Countries>();
            var random = new Random();
            var employees = rootData.Where(x => x.EmployeeID == random.Next(1, 10)).GroupBy(x => x.ShipCity).Select(y => y.First()).ToList<RootTree>();
           
            foreach (var item in employees)
            {
                var child = new Countries() { Name = item.ShipCity };

                //Added CheckedItems to check multiple items through binding.
                if (item.Verified == "true")
                {
                    CheckedItems.Add(child);
                }
                details.Add(child);
            }

            return details;
        }

        /// <summary>
        ///  Process the fetched data for parent items. 
        /// </summary>
        /// <param name="rootData">Data fetched from the web service.</param>
        /// <returns>Returns the processed data for parent nodes.</returns>
        internal async Task<ObservableCollection<Countries>> ProcessParentNodes(ObservableCollection<RootTree> rootData)
        {
            var countries = rootData.GroupBy(x => x.ShipCountry).Select(y => y.First()).OrderBy(z => z.EmployeeID);
            var countryInfo = new ObservableCollection<Countries>();
            if (countries != null && countries.Count() > 0)
            {
                foreach (var item in countries)
                {
                    var country = new Countries() { Name = item.ShipCountry, ID = item.EmployeeID, HasChildNodes = true};
                    countryInfo.Add(country);
                }
            }
            return countryInfo;
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}