using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace TreeViewXamarin
{
    #region Countries
    public class Countries : INotifyPropertyChanged
    {

        public int ID { get; set; }
       
        public string Name { get; set; }

        public ObservableCollection<Countries> States { get; set; }

        private bool hasChildNodes;

        public bool HasChildNodes
        {
            get { return hasChildNodes; }
            set
            {
                hasChildNodes = value;
                OnPropertyChanged("HasChildNodes");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public Countries()
        {
            States = new ObservableCollection<Countries>();
        }
    }
    #endregion

    #region RootTreeModel
    public class RootTree
    {
        public int OrderID { get; set; }
        public int EmployeeID { get; set; }
        public string CustomerID { get; set; }
        public string ShipName { get; set; }
        public string ShipCountry { get; set; }
        public string ShipCity { get; set; }
        public string Verified { get; set; }
    }
    #endregion
}