using System;
using System.Collections.Generic;
using System.Text;
using OrderFormSystem.DB;

namespace OrderFormSystem
{
    class MainOrder
    {

    }

    class Order
    {
        private const int MAX_STEP_COUNT = 14;
        private const int MAX_PRESCRIPTION_COUNT = 9;

        public string m_orderID;
        public float m_liquorRatio1;
        public int m_yarnCount;
        public string m_machineCode;
        public string m_colorNumber;
        public int m_creelCount;
        public float m_yarnWeight;
        public float m_liquorRatio2;
        public float m_waterWight;

        public string m_prescriptionCode;
        public string m_pattern;
        public string m_storeTime;

        public int m_stepCount;
        public string[] m_stepCodes = new string[14];

        public int m_prescriptionCount;
        public string[] m_dyeCodes = new string[9];
        public float[] m_density = new float[9];
        public string[] m_dyeNames = new string[9];
        public int[] m_weight = new int[9];
        public int[] m_valve = new int[9];

        private List<TableElement> m_elements;


        public Order(string orderID,
            float liquorRatio1,
            int yarnCount,
            string machineCode,
            string colorNumber,
            int creelCount,
            float yarnWeight,
            float liquorRatio2,
            float waterWeight,

            string prescriptionCode,
            string pat,
            string storTime,

            int stepCount,
            string[] stepCodes,

            int prescriptionCount,
            string[] dyeCodes,
            float[] density
            )
        {
            m_orderID = orderID;
            m_liquorRatio1 = liquorRatio1;
            m_yarnCount = yarnCount;
            m_machineCode = machineCode;
            m_colorNumber = colorNumber;
            m_creelCount = creelCount;
            m_yarnWeight = yarnWeight;
            m_liquorRatio2 = liquorRatio2;
            m_waterWight = waterWeight;

            m_prescriptionCode = prescriptionCode;
            m_pattern = pat;
            m_storeTime = storTime;

            m_stepCount = stepCount;
            for (int i = 0; i < m_stepCount; i++)
            {
                m_stepCodes[i] = stepCodes[i];
            }
            m_prescriptionCount = prescriptionCount;
            for (int i = 0; i < m_prescriptionCount; i++)
            {
                m_dyeCodes[i] = dyeCodes[i];
                m_density[i] = density[i];
                m_weight[i] = (int)(m_density[i] * m_liquorRatio2 * m_yarnWeight);
                Dye dye = DyeDB.GetRecord(m_dyeCodes[i]);
                if (dye != null)
                {
                    m_dyeNames[i] = dye.DyeName;
                    m_valve[i] = dye.DyeValve;
                }                
            }

            m_elements = new List<TableElement>();
            m_elements.Add(new TableElement("OrderID", m_orderID));
            m_elements.Add(new TableElement("LiquorRatio1", m_liquorRatio1));
            m_elements.Add(new TableElement("YarnCount", m_yarnCount));
            m_elements.Add(new TableElement("MachineCode", m_machineCode));
            m_elements.Add(new TableElement("ColorNumber", m_colorNumber));
            m_elements.Add(new TableElement("CreelCount", m_creelCount));
            m_elements.Add(new TableElement("YarnWeight", m_yarnWeight));
            m_elements.Add(new TableElement("LiquorRatio2", m_liquorRatio2));
            m_elements.Add(new TableElement("WaterWeight", m_waterWight));
            m_elements.Add(new TableElement("PrescriptionCode", m_prescriptionCode));
            m_elements.Add(new TableElement("Pattern", m_pattern));
            m_elements.Add(new TableElement("StoreTime", m_storeTime));

            m_elements.Add(new TableElement("StepCount", m_stepCount));
            for (int i = 0; i < m_stepCount; i++ )
            {
                int index = i+1;
                m_elements.Add(new TableElement("StepCode" + index.ToString(), m_stepCodes[i]));
            }
            for (int i = m_stepCount; i < MAX_STEP_COUNT; i++)//填充
            {
                int index = i + 1;
                m_elements.Add(new TableElement("StepCode" + index.ToString(), ""));
            }

            m_elements.Add(new TableElement("PrescriptionCount", m_prescriptionCount));
            for (int i = 0; i < m_prescriptionCount; i++)
            {
                int index = i + 1;
                m_elements.Add(new TableElement("DyeCode" + index.ToString(), m_dyeCodes[i]));
                m_elements.Add(new TableElement("Density" + index.ToString(), m_density[i]));
            }
            for (int i = m_prescriptionCount; i < MAX_PRESCRIPTION_COUNT; i++)//填充
            {
                int index = i + 1;
                m_elements.Add(new TableElement("DyeCode" + index.ToString(), ""));
                m_elements.Add(new TableElement("Density" + index.ToString(), 0.0f));
            }
        }

        public Order(List<TableElement> elements)
        {
            m_orderID = (string)elements[0].Value;
            m_liquorRatio1 = (float)Convert.ToDouble(elements[1].Value);
            m_yarnCount = Convert.ToInt32(elements[2].Value);
            m_machineCode = (string)elements[3].Value;
            m_colorNumber = (string)elements[4].Value;
            m_creelCount = Convert.ToInt32(elements[5].Value);
            m_yarnWeight = (float)Convert.ToDouble(elements[6].Value);
            m_liquorRatio2 = (float)Convert.ToDouble(elements[7].Value);
            m_waterWight = (float)Convert.ToDouble(elements[8].Value);

            m_prescriptionCode = (string)elements[9].Value;
            m_pattern = (string)elements[10].Value;
            m_storeTime = (string)elements[11].Value;

            m_stepCount = (int)elements[12].Value;
            for (int i = 0; i < m_stepCount; i++ )
            {
                m_stepCodes[i] = elements[12+ 1 +i].Value == null ? String.Empty : (string)elements[12 + 1+i].Value;
            }

            m_prescriptionCount = (int)elements[12 + MAX_STEP_COUNT + 1].Value;
            for (int i = 0; i < m_prescriptionCount; i++)
            {
                m_dyeCodes[i] = elements[12 + MAX_STEP_COUNT + 1 + 1 + i * 2].Value == null ? String.Empty : (string)elements[12 + MAX_STEP_COUNT + 1 + 1 + i * 2].Value;
                m_density[i] = elements[12 + MAX_STEP_COUNT + 1 + 2 + i * 2].Value == null ? -1 : (float)Convert.ToDouble(elements[12 + MAX_STEP_COUNT + 1 + 2 + i * 2].Value);
            }
        }

        public List<TableElement> Elements
        {
            get { return m_elements; }
        }
    }
}
