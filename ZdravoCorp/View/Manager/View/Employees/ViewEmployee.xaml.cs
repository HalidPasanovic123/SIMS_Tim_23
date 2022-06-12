﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoCorp.View.Manager.ViewModel.Employees;

namespace ZdravoCorp.View.Manager.View.Employees
{
    /// <summary>
    /// Interaction logic for ViewEmployee.xaml
    /// </summary>
    public partial class ViewEmployee : UserControl
    {
        public ViewEmployee(ViewEmployeeViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
