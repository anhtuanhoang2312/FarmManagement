using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmManagement.Class
{
    public class Notify : INotifyPropertyChanged
    {
        private bool _categoryChange = false;

        public bool CategoryChange
        {
            get => _categoryChange;
            set
            {
                _categoryChange = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("_categoryChange"));
                }
            }
        }

        private bool _productChange = false;

        public bool ProductChange
        {
            get => _productChange;
            set
            {
                _productChange = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("_productChange"));
                }
            }
        }

        private bool _customerChange = false;

        public bool CustomerChange
        {
            get => _customerChange;
            set
            {
                _customerChange = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("_customerChange"));
                }
            }
        }

        private bool _invoiceDetailChange = false;

        public bool InvoiceDetailChange
        {
            get => _invoiceDetailChange;
            set
            {
                _invoiceDetailChange = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("_invoiceDetailChange"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
