using Calculator;
using CalculatorLib;
using System;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Calculator
{
    public class MainWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        private Thread _displayThread;
        private bool _hasCalculated = false;

        private string _currentCalculation = "";
        public string CurrentCalculation
        {
            get 
            {
                return _currentCalculation;
            }
            set
            {
                _currentCalculation = value;
                OnPropertyChanged(nameof(CurrentCalculation));
            }
        }

        private string _currentValue = "0";
        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue == value) return;

                _currentValue = value;
                OnPropertyChanged(nameof(CurrentValue));
            }
        }

        public string MemoryValue
        {
            get => $"{_calculatorMemory.Recall()}";
        }

        private bool _displayPopUp;

        public bool DisplayPopUp
        {
            get => _displayPopUp;
            set
            {
                if (_displayPopUp == value) return;

                _displayPopUp = value;
                OnPropertyChanged(nameof(DisplayPopUp));
            }
        }

        private CalculatorEngine _calculationEngine;

        private CalculatorMemory _calculatorMemory;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            _calculationEngine = new CalculatorEngine();
            _calculatorMemory = new CalculatorMemory();

            _keyCommand = new SimpleCommand(HandleKeyCommand);
            _clearCommand = new SimpleCommand(HandleClearCommand);
            _clearEntryCommand = new SimpleCommand(HandleClearEntryCommand);
            _memoryViewClosed = new SimpleCommand(HandleMemoryViewClosedCommand);

            _displayThread = new Thread(() =>
            {
                Thread.Sleep(5000);
                DisplayPopUp = false;
            })
            { IsBackground = true };
        }

        private void HandleClearEntryCommand(object obj)
        {
            CurrentValue = "0";
        }

        private void HandleClearCommand(object obj)
        {
            CurrentCalculation = string.Empty;
            CurrentValue = "0";
        }

        private void HandleKeyCommand(object value)
        {
            if (_hasCalculated)
            {
                CurrentValue = "0";
                CurrentCalculation = string.Empty;

                _hasCalculated = false;
            }

            if (DisplayPopUp) DisplayPopUp = false;

            switch (value)
            {
                case "BSpc":
                    CurrentValue = CurrentValue.Substring(0, CurrentValue.Length - 1);
                    if (string.IsNullOrWhiteSpace(CurrentValue)) CurrentValue = "0";

                    break;
                case "=":
                    CurrentCalculation = $"{CurrentCalculation}{CurrentValue}";

                    CurrentValue = $"{_calculationEngine.ProcessCalculation(CurrentCalculation)}";
                    _hasCalculated = true;

                    break;

                case "MC":
                    _calculatorMemory.Clear();
                    OnPropertyChanged(nameof(MemoryValue));

                    break;

                case "MR":
                    CurrentValue = $"{_calculatorMemory.Recall()}";
                    OnPropertyChanged(nameof(MemoryValue));
                    break;

                case "M+":
                    if (!double.TryParse(CurrentValue, out double addResult)) break;

                    _calculatorMemory.AddValue(addResult);
                    OnPropertyChanged(nameof(MemoryValue));
                    break;

                case "M-":
                    if (!double.TryParse(CurrentValue, out double minusResult)) break;

                    _calculatorMemory.SubtractValue(minusResult);
                    OnPropertyChanged(nameof(MemoryValue));
                    break;

                case "MS":
                    if (!double.TryParse(CurrentValue, out double storeResult)) break;

                    _calculatorMemory.StoreValue(storeResult);
                    OnPropertyChanged(nameof(MemoryValue));
                    break;

                case "MV":
                    DisplayPopUp = true;
                    _displayThread.Start();

                    OnPropertyChanged(nameof(MemoryValue));

                    break;

                case "+":
                    CurrentCalculation = $"{CurrentCalculation}{CurrentValue} + ";
                    CurrentValue = string.Empty;

                    break;

                case "-":
                    CurrentCalculation = $"{CurrentCalculation}{CurrentValue} - ";
                    CurrentValue = string.Empty;
                    break;

                case "/":
                    CurrentCalculation = $"{CurrentCalculation}{CurrentValue} / ";
                    CurrentValue = string.Empty;
                    break;

                case "*":
                    CurrentCalculation = $"{CurrentCalculation}{CurrentValue} * ";
                    CurrentValue = string.Empty;
                    break;

                default:
                    if (_currentValue == "0")
                        _currentValue = string.Empty;

                    CurrentValue = $"{CurrentValue}{value}";

                    break;  
            }

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand _keyCommand;
        public ICommand KeyCommand
        {
            get => _keyCommand;
        }

        private ICommand _clearEntryCommand;
        public ICommand ClearEntryCommand
        {
            get => _clearEntryCommand;
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get => _clearCommand;
        }

        public void HandleMemoryViewClosedCommand(object obj)
        {
            DisplayPopUp = false;
        }

        private ICommand _memoryViewClosed;
        public ICommand MemoryViewClosed
        {
            get => _memoryViewClosed;
        }
    }
}
