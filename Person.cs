using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM_7
{
    public class Person : INotifyPropertyChanged
    {
        public int Id { get; set; }
        string? namePerson;
        string? phone;

        public string? NamePerson
        {
            get { return namePerson; }
            set
            {
                namePerson = value;
                OnPropertyChanged("NamePerson");
            }
        }
        public string?  Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string ToCVS()
        {
            return $"{Id};{NamePerson};{Phone}";
        }

        public override string ToString()
        {
            return $" Id = {Id}, NamePerson = {NamePerson}, Phone = {Phone} ";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
