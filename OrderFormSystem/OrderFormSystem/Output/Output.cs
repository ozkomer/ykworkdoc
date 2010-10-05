using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFormSystem.Output
{
    class Output
    {
        protected Order m_order;

        public Output()
        {
        }

        public Output(Order od)
        {
            m_order = od;
        }

        public virtual void PrintOut()
        {

        }
    }
}
