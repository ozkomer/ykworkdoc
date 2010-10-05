using System;
using System.Collections.Generic;
using System.Text;

namespace OrderFormSystem
{
    public enum DyeType
    {
        DYE = 0,
        AUXILIARY = 1
    }

    class Dye
    {
        private string m_dyeCode;
        private DyeType m_dyeType;
        private string m_dyeName;
        private int m_dyeValve;
        private List<TableElement> m_elements;

        public Dye(string dyeCode, DyeType dt, string dyeName, int dyeValve)
        {
            m_dyeCode = dyeCode;
            m_dyeType = dt;
            m_dyeName = dyeName;
            m_dyeValve = dyeValve;

            TableElement t1 = new TableElement("DyeCode",dyeCode);
            TableElement t2 = new TableElement("DyeType", dt);
            TableElement t3 = new TableElement("DyeName", dyeName);
            TableElement t4 = new TableElement("DyeValve", dyeValve);
            m_elements = new List<TableElement>();
            m_elements.Add(t1);
            m_elements.Add(t2);
            m_elements.Add(t3);
            m_elements.Add(t4);
        }

        public Dye(List<TableElement> elements)
        {
            m_dyeCode = (string)elements[0].Value;
            m_dyeType = (DyeType)(Convert.ToInt32(elements[1].Value));
            m_dyeName = (string)elements[2].Value;
            m_dyeValve = Convert.ToInt32(elements[3].Value);

            m_elements = elements;
        }

        #region public properties
        public List<TableElement> Elements
        {
            get { return m_elements; }
            //set {m_elements = value;}
        }

        public string DyeCode
        {
            get { return m_dyeCode; }
            //set { m_dyeCode = value; }
        }

        public DyeType DyeType
        {
            get { return m_dyeType; }
            //set { m_dyeType = value; }
        }

        public string DyeName
        {
            get { return m_dyeName; }
            //set { m_dyeName = value; }
        }

        public int DyeValve
        {
            get { return m_dyeValve; }
            //set { m_dyeValve = value; }
        }
        #endregion

        #region public methods

        #endregion
    }
}
