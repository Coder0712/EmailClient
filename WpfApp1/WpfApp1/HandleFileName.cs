using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class HandleFileName
    {
        public string FileName { get; set; }

        // String automatisch zu HandleFileName umwandeln
        public static implicit operator HandleFileName (string str)
        {
            return new HandleFileName() { FileName = str};
        }
    }
}
