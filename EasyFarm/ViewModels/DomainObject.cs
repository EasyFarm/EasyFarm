using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;

public abstract class DomainObject : INotifyPropertyChanged,
                                     INotifyDataErrorInfo
{
    private ErrorsContainer<ValidationResult> errorsContainer;

    public DomainObject()
    {
        errorsContainer = new ErrorsContainer<ValidationResult>(pn => this.RaiseErrorsChanged(pn));
    }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors
    {
        get { return this.errorsContainer.HasErrors; }
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return this.errorsContainer.GetErrors(propertyName);
    }

    protected void RaiseErrorsChanged(string propertyName)
    {
        var handler = this.ErrorsChanged;
        if (handler != null)
        {
            handler(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    event System.EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
    {
        add { throw new System.NotImplementedException(); }
        remove { throw new System.NotImplementedException(); }
    }

    System.Collections.IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
    {
        throw new System.NotImplementedException();
    }
}
