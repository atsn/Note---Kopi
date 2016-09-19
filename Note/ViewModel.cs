using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Eventmaker.Common;
using Note;
using NoteMvvmPersistency.Persistency;


namespace NoteMVVM
{



    class ViewModel : INotifyPropertyChanged
    {
        #region inotify
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion



        public ObservableCollection<NoteModel> ToDo { get; set; }
        public ObservableCollection<NoteModel> Done { get; set; }
        public ObservableCollection<ObservableCollection<NoteModel>> list { get; set; }
        public string emne { get; set; }
        public string tekst { get; set; }

        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand DoneCommand { get; set; }
        public RelayCommand ToDoCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand ClearDoneCommand { get; set; }

        public int temp { get; set; }
        public object index { get; set; }






        public void add()
        {
            ToDo.Add(new NoteModel(emne, tekst));

        }

        public void remove()
        {
            foreach (var note in ToDo)
            {
                if (index == note)
                {
                    ToDo.Remove(note);
                    break;
                }
            }
            foreach (var note in Done)
            {
                if (index == note)
                {
                    Done.Remove(note);
                    break;
                }
            }
            //            foreach (var note in ToDo)
            //            {
            //                if (note.Nummer == temp)
            //                {
            //                    ToDo.Remove(note);
            //                    break;
            //                }

            //;

            //            }

        }

        public void MoveToDone()
        {
            foreach (var note in ToDo)
            {
                if (index == note)
                {

                    Done.Add(note);
                    ToDo.Remove(note);
                    break;
                }
            }

        }




        public void MoveToToDo()
        {
            foreach (var note in Done)
            {
                if (index == note)
                {
                    ToDo.Add(note);
                    Done.Remove(note);
                    break;
                }
            }

        }


        public ViewModel()
        {

            list = new ObservableCollection<ObservableCollection<NoteModel>>();
            Done = new ObservableCollection<NoteModel>();
            ToDo = new ObservableCollection<NoteModel>();
            list.Add(ToDo);
            list.Add(Done);
            RemoveCommand = new RelayCommand(remove);
            DoneCommand = new RelayCommand(MoveToDone);
            ToDoCommand = new RelayCommand(MoveToToDo);
            AddCommand = new RelayCommand(add);
            SaveCommand = new RelayCommand(SaveNotes);
            LoadCommand = new RelayCommand(LoadNotes);
            ClearCommand = new RelayCommand(clearall);
            ClearDoneCommand = new RelayCommand(clearDone);

            //ToDo.Add(new NoteModel("hej", "anders"));
            //Done.Add(new NoteModel("hej", "anders"));
            //ToDo.Add(new NoteModel("køb", "mælk"));
        }








        private async void LoadNotes()
        {
            var notes = await PersistencyService.LoadNotesFromJsonAsync();
            if (notes != null)
            {


                Done.Clear();
                ToDo.Clear();
                ObservableCollection<NoteModel> NewDone = (ObservableCollection<NoteModel>)notes[1];
                ObservableCollection<NoteModel> NewToDo = (ObservableCollection<NoteModel>)notes[0];

                foreach (var note in NewDone)
                {
                    Done.Add(note);

                }
                foreach (var note in NewToDo)
                {
                    ToDo.Add(note);
                }

            }

        }


        private async void SaveNotes()
        {

            PersistencyService.SaveNotesAsJsonAsync(list);

        }

        private void clearall()
        {
            Done.Clear();
            ToDo.Clear();
        }

        private void clearDone()
        {
            Done.Clear();
        }


    }

    internal class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
    }
}
