using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls.Ribbon.Primitives;

namespace biblioteca.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<object> Tabs { get; set; }

        public MainViewModel()
        {
            Tabs = new ObservableCollection<object>
            {
                new BookListViewModel(),
                new UserListViewModel(),
                new ReturnBooksViewModel(),
                new BorrowingHistoryViewModel()
            };
        }
    }
}
