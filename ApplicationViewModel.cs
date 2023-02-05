using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using LoggerClass;

namespace MVVM_7
{
    public class ApplicationViewModel
    {
        static IServiceCollection services = new ServiceCollection().AddTransient<ILogger, WriteToFile>();
        static ServiceProvider serviceProvider = services.BuildServiceProvider();
        ILogger logService = serviceProvider.GetService<ILogger>();

        ApplicationContext db = new ApplicationContext();
        RelayCommand? addCommand;
        RelayCommand? editCommand;
        RelayCommand? deleteCommand;
        public ObservableCollection<Person> Persons { get; set; }
        public ApplicationViewModel()
        {
            db.Database.EnsureCreated();
            db.Persons.Load();
            Persons = db.Persons.Local.ToObservableCollection();
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      PersonEdit personEdit = new PersonEdit(new Person());
                      if (personEdit.ShowDialog() == true)
                      {
                          Person person = personEdit.Person;
                          db.Persons.Add(person);
                          db.SaveChanges();
                          logService.WriteLog($"Add to collection {person}");
                      }
                  }));
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      Person? person = selectedItem as Person;
                      if (person == null) return;

                      Person vm = new Person
                      {
                          Id = person.Id,
                          NamePerson = person.NamePerson,
                          Phone = person.Phone
                      };
                      PersonEdit userWindow = new PersonEdit(vm);


                      if (userWindow.ShowDialog() == true)
                      {
                          person.NamePerson = userWindow.Person.NamePerson;
                          person.Phone = userWindow.Person.Phone;
                          db.Entry(person).State = EntityState.Modified;
                          db.SaveChanges();
                          logService.WriteLog($"Edit {person}");
                      }
                  }));
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      Person? person = selectedItem as Person;
                      if (person == null) return;

                      if (System.Windows.MessageBox.Show("Do you want to delete?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                      {
                              db.Persons.Remove(person);
                              db.SaveChanges();
                          logService.WriteLog($"Delete {person}");
                      }
                  }));
            }
        }
    }
}
