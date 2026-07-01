using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VehicleRegistryManager.Infrastructure.BaseUI;

public class BaseNotifyPropertyChanged: INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual bool SetProperty<T>(
           ref T backingStore, T value,
           [CallerMemberName] string propertyName = "",
           Action? onChanged = null,
           Func<T, T, bool>? validateValue = null)
    {
        //if value didn't change
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        //if value changed but didn't validate
        if (validateValue != null && !validateValue(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }
}

