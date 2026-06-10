using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using biblioteca.ViewModels;
using biblioteca.Models;

namespace biblioteca.Views
{
    public partial class ReturnBooks : UserControl
    {
        public ReturnBooks()
        {
            InitializeComponent();
        }

        private void LoansListBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DataContext is ReturnBooksViewModel vm)
            {
                vm.SelectedLoans.Clear();

                foreach (Loan loan in LoansListBox.SelectedItems)
                {
                    vm.SelectedLoans.Add(loan);
                }
            }
        }
    }
}