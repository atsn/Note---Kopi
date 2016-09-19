using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Newtonsoft.Json;
using NoteMVVM;

namespace NoteMvvmPersistency.Persistency
{
    //Json.Net er downloaded til projektet via NuGet: Højreklik på projektet -> Manage NuGet Package

    class PersistencyService
    {
        private static string JsonFileName = "NotesAsJson.dat";

        public static async void SaveNotesAsJsonAsync(ObservableCollection<ObservableCollection<NoteModel>> notes)
        {
            string notesJsonString = JsonConvert.SerializeObject(notes);
            SerializeNotesFileAsync(notesJsonString, JsonFileName);
        }

        public static async Task<ObservableCollection<ObservableCollection<NoteModel>>> LoadNotesFromJsonAsync()
        {
            string notesJsonString = await DeserializeNotesFileAsync(JsonFileName);
            if (notesJsonString != null)
                return (ObservableCollection<ObservableCollection<NoteModel>>) JsonConvert.DeserializeObject(notesJsonString, typeof (ObservableCollection<ObservableCollection<NoteModel>>));
            return null;
        }

       

        private static async void SerializeNotesFileAsync(string notesJsonString, string fileName)
        {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, notesJsonString);
        }

        
        private static async Task<string> DeserializeNotesFileAsync(string fileName)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localFile);
            }
            catch (FileNotFoundException ex)
            {
                MessageDialogHelper.Show("Loading for the first time? - Try Add and Save some Notes before trying to Save for the first time", "File not Found");
                return null;
            }
        }


        private class MessageDialogHelper
        {
            public static async void Show(string content, string title)
            {
                MessageDialog messageDialog = new MessageDialog(content, title);
                await messageDialog.ShowAsync();
            }
        }

    }
}
