using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoNow
{
    public class Memo : INotifyPropertyChanged
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Number
        {
            get;
            set;
        }

        [NotNull]
        public string Title
        {
            get;
            set;
        }
        
        public string Memo_Date
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
