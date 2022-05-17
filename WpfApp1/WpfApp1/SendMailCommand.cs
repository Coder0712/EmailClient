using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    public class SendMailCommand : ICommand
    {
        // Klasse wird auch DelegateCOmmand genannt

        // gibt void zurück und bekommt einen Parameter übergeben
        // bildet die Execute Methode ab
        // damit man diese von außen aufrufen kann
        // ist letztendlich nur eine Variable
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        // bildet die CanExecute ab
        // Predicate da ein bool zurückgegeben werden soll
        // ist letztendlich nur eine Variable


        /* Kurzschreibweise des Konstruktors:
         * => (this.execute, this.canExecute) = (execute, canExecute)
         */

        public SendMailCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public SendMailCommand(Action<object> execute) { }


        /* Bedeutung ??
         * Wenn CanExecute nicht gesetzt ist soll es immer true zurückgeben
         * Kurzschreibweise für Funktionszuweisung (ähnlich zu javascript) ist ein lambda
         */
        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? true;

        // parameter in invoke ist object, welches dieses Execute aufgerufen hat
        // Kurzschreibweise für Funktionszuweisung (ähnlich zu javascript) ist ein lambda
        public void Execute(object parameter) => this.execute?.Invoke(parameter);


        // da ein Event nur innerhalb einer Klasse aufgerufen werden kann
        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);


        public event EventHandler CanExecuteChanged;

    }
}
