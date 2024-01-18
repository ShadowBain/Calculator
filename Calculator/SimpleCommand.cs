using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object> function;

        public SimpleCommand(Action<object> func)
        {
            function = func;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter == null) return;

            function.Invoke(parameter);
        }
    }
}
