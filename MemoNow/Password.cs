using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoNow
{
    public class Password : INotifyPropertyChanged
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        [MaxLength(10)]
        public string Psw
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
