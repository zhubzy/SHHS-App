using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace SHHS.Model
{
    public class SHHSEventManager
    {

        private SQLiteAsyncConnection _connection;
        public ObservableCollection<SHHSEvent> events;


        public SHHSEventManager()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");
            _connection = new SQLiteAsyncConnection(databasePath);


        }
        public async Task InitalizeEventTable()
        {

            await _connection.CreateTableAsync<SHHSEvent>();
            var data = await _connection.Table<SHHSEvent>().ToListAsync();
            events = new ObservableCollection<SHHSEvent>(data);
            events.CollectionChanged += Events_CollectionChanged;

         
        }

        public async Task AddEvent(SHHSEvent e) {

            await _connection.InsertAsync(e);
            events.Add(e);

        }

        public async Task RemoveEvent(SHHSEvent e)
        {

            await _connection.DeleteAsync(e);
            events.Remove(e);

        }


        public void RefreshEvent() { 
        
            foreach(var a in events) {

              
               
           }





        }









        void Events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) { 


            }

        }

    }
}
