using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace ReadWriteFiles
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Person> personlist;
        public MainPage()
        {
            this.InitializeComponent();

        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            personlist = await LoadList("file.json");
            this.list.ItemsSource = new ObservableCollection<Person>(personlist);
            base.OnNavigatedTo(e);
        }
        public async void SaveList(string filename) {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(filename, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            var jsonlist = JsonConvert.SerializeObject(personlist);
            await Windows.Storage.FileIO.WriteTextAsync(file, jsonlist);
        }
        public async Task<List<Person>> LoadList(string filename) {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file;
            try
            {
                file = await folder.GetFileAsync(filename);
            }
            catch (Exception )
            {
                return new List<Person>();
            }
            var jsonlist = await Windows.Storage.FileIO.ReadTextAsync(file);
            List<Person> list = JsonConvert.DeserializeObject<List<Person>>(jsonlist);
            return list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var person = new Person() { 
                Name = name.Text,
                Age = int.Parse(age.Text)
            };
            personlist.Add(person);
            this.list.ItemsSource = new ObservableCollection<Person>(personlist);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveList("file.json");
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
