# fetch-data-using-webapi-treeview-xamarin
This repository showcases how to populate the Syncfusion Xamarin.Forms TreeView control with hierarchical data fetched from a Web API. The included sample demonstrates making HTTP requests to retrieve JSON data, deserializing the response into a suitable object hierarchy, and binding the result to the TreeView.

## Sample

### XAML
```xaml
<syncfusion:SfTreeView x:Name="treeView" 
                               ItemTemplateContextType="Node" 
                               ItemsSource="{Binding CountriesInfo}"
                               CheckBoxMode="Recursive"
                               CheckedItems="{Binding CheckedItems}"
                               LoadOnDemandCommand="{Binding TreeViewOnDemandCommand}">
            <syncfusion:SfTreeView.ItemTemplate>
                <DataTemplate>
                    <Grid Padding="5">
                        <SfButtons:SfCheckBox x:Name="CheckBox"
                                              Text="{Binding Content.Name}"
                                              IsChecked="{Binding IsChecked, Mode=TwoWay}"
                                              VerticalOptions="CenterAndExpand"/>
                    </Grid>
                </DataTemplate>
            </syncfusion:SfTreeView.ItemTemplate>
        </syncfusion:SfTreeView>
```

### Helper
```csharp
internal class WebApiServices
    {
        public static string webApiUrl = "https://ej2services.syncfusion.com/production/web-services/api/Orders"; // Your Web Api here

        System.Net.Http.HttpClient client;

        public WebApiServices()
        {
            client = new System.Net.Http.HttpClient();
        }

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
    }
```

## Requirements to run the demo

To run the demo, refer to [System Requirements for Xamarin](https://help.syncfusion.com/xamarin/system-requirements)

## Troubleshooting
### Path too long exception
If you are facing path too long exception when building this example project, close Visual Studio and rename the repository to short and build the project.
