using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace OrderFormSystem.Output
{
    class TxtOutput : Output
    {
        public TxtOutput(Order od)
        {
            m_order = od;
        }

        public override void PrintOut()
        {
            string totalText = "订单号: " + m_order.m_orderID + "\n"
                + "浴比: " + m_order.m_liquorRatio1 + "\n"
                + "纱支: " + m_order.m_yarnCount + "\n"
                + "染机代码: " + m_order.m_machineCode + "\n"
                + "色号: " + m_order.m_colorNumber + "\n"
                + "筒子个数: " + m_order.m_creelCount + "\n"
                + "纱重: " + m_order.m_yarnWeight + "\n"
                + "浴比: " + m_order.m_liquorRatio2 + "\n"
                + "水位: " + m_order.m_waterWight + "\n"
                + "步骤数: " + m_order.m_stepCount + "\n"
                ;

            string stepText = "";
            for (int i = 0; i<m_order.m_stepCount; i++)
            {
                int index = i+1;
                stepText += "步骤" + index.ToString() + ": "+ m_order.m_stepCodes[i] + "\n";
            }

            string textPrescription = "配方代码: " + m_order.m_prescriptionCode + "\n"
                + "花型: " + m_order.m_pattern + "\n"
                + "存档时间: " + m_order.m_storeTime + "\n"
                ;

            string textDetails = "";
            for (int i = 0; i < m_order.m_prescriptionCount; i++)
            {
                int weight = (int) (m_order.m_density[i] * m_order.m_yarnWeight * m_order.m_liquorRatio2);
                textDetails += "染助剂代码: " + m_order.m_dyeCodes[i] + "   "
                    + "浓度: " + m_order.m_density[i] + "   "
                    + "重量: " + weight.ToString() + "   "
                    + "阀号: " + m_order.m_valve[i] + "   "
                    + "染助剂名称: " + m_order.m_dyeNames[i]
                    + "\n"
                    ;
            }

            string text = totalText + stepText + textPrescription +textDetails;

            File.WriteAllText("xxxx.txt", text);
        }
    }
}
