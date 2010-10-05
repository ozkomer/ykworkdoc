using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFormSystem
{
    public class TableElement
    {
        private string m_element;
        private object m_value;

        public TableElement(string e, object v)
        {
            m_element = e;
            m_value = v;
        }

        public string Element
        {
            get {return  m_element;}
            set {m_element = value;}
        }

        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
    }
}
