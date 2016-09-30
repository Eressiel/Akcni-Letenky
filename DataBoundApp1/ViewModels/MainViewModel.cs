using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DataBoundApp1.Resources;
using System.Net;
using Newtonsoft.Json;
using DataBoundApp1.Model;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Linq;
using DataBoundApp1.ViewModels;


namespace DataBoundApp1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        const string apiUrl = @"http://akcniletenky.azurewebsites.net/api/";

        private static string itemsFileName = "items.xml";

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        private ObservableCollection<ItemViewModel> items;
        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return items;
            }
            set
            {
                if (value != items)
                {
                    items = value;
                    IsItemsEmpty = items.Count == 0 ? false : true;
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        private bool isItemsEmpty;
        public bool IsItemsEmpty
        {
            get
            {
                return isItemsEmpty;
            }
            set
            {
                isItemsEmpty = value;
                NotifyPropertyChanged("IsItemsEmpty");
            }
        }

        public async Task LoadData()
        {
            await LoadItemsAsync();
            var titles = await DownloadNewTitlesAndDeserializeAsync();

            var id = GetHighestId(Items);
            AddTitlesToItems(titles, id);
            RemoveOutdatedItems();
            await SaveItemsAsync();
            this.IsDataLoaded = true;
        }


        private async Task<List<TitleDetails>> DownloadNewTitlesAndDeserializeAsync()
        {
            var downloadedTitles = await DownloadNewItemsFromServerAsync();
            return await DeserializeJsonItemsAsync(downloadedTitles);
        }

        private async Task<string> DownloadNewItemsFromServerAsync()
        {
            var httpClient = new HttpClient();
            string downloadedTitles = String.Empty;
            if (Items.Count == 0)
            {
                downloadedTitles = await httpClient.GetStringAsync(new Uri(apiUrl + "titles"));
            }
            else
            {
                DateTime latestDate = Items.Max(t => t.Date);
                var tst = latestDate.ToString("yyyyMMddHHmmss");
                downloadedTitles = await httpClient.GetStringAsync(new Uri(apiUrl + "titles/" + latestDate.ToString("yyyyMMddHHmmss")));
            }
            return downloadedTitles;
        }

        private void RemoveOutdatedItems()
        {
            DateTime oldDate = DateTime.Now.AddDays(-14);
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].Date < oldDate)
                {
                    Items.RemoveAt(i);
                }
            }
        }

        private async Task LoadItemsAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                // Getting JSON from file if it exists, or file not found exception if it does not
                StorageFile textFile = await localFolder.GetFileAsync(itemsFileName);

                using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
                {
                    using (DataReader textReader = new DataReader(textStream))
                    {
                        //get size
                        uint textLength = (uint)textStream.Size;
                        await textReader.LoadAsync(textLength);

                        string jsonContents = textReader.ReadString(textLength);

                        List<TitleDetails> titles = await DeserializeJsonItemsAsync(jsonContents);
                        foreach (var title in titles)
                        {
                            Items.Add(new ItemViewModel()
                            {
                                Id = title.Id,
                                ArticleTitle = title.ArticleTitle,
                                Date = title.Date,
                                Url = title.Url,

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task SaveItemsAsync()
        {
            string jsonContents = JsonConvert.SerializeObject(Items);

            // Get the app data folder and create or replace the file we are storing the JSON in.
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.CreateFileAsync(itemsFileName, CreationCollisionOption.ReplaceExisting);

            using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (DataWriter textWriter = new DataWriter(textStream))
                {
                    textWriter.WriteString(jsonContents);
                    await textWriter.StoreAsync();
                }
            }
        }

        private int GetHighestId(List<TitleDetails> titles)
        {
            if (titles.Count != 0)
            {
                var res = titles.Max(t => t.Id);
                return res;
            }
            return 0;
        }

        private int GetHighestId(ObservableCollection<ItemViewModel> items)
        {
            if (items.Count != 0)
            {
                var res = items.Max(t => t.Id);
                return res;
            }
            return 0;
        }

        private void AddTitlesToItems(List<TitleDetails> titles, int startingId)
        {
            foreach (var title in titles)
            {
                Items.Add(new ItemViewModel()
                {
                    Id = (++startingId),
                    ArticleTitle = title.ArticleTitle,
                    Date = title.Date,
                    Url = title.Url,

                });
            }
        }

        private async Task<string> SerializeJsonItemsAsync(List<TitleDetails> items)
        {
            var titles = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(items));
            return titles;
        }

        private async Task<List<TitleDetails>> DeserializeJsonItemsAsync(string titlesJsonString)
        {
            var titles = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<TitleDetails>>(titlesJsonString));
            return titles;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}