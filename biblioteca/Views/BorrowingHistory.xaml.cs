using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using biblioteca.ViewModels;

namespace biblioteca.Views
{
    public partial class BorrowingHistory : UserControl
    {
        public BorrowingHistory()
        {
            InitializeComponent();
            DataContext = new BorrowingHistoryViewModel();
        }
    }
}