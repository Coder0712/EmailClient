using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class KeyVal<Key, Val>
    {
        public Key id { get; set; }
        public Val value { get; set; }

        public string Name { get; set; }

        // leerer Kontruktor
        public KeyVal()
        {

        }

        // zweiter Konsruktor um werte zuzuweisen
        public KeyVal(Key key, Val value)
        {
            this.id = key;
            this.value = value;
        }
    }
}
