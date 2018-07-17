using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaoJin.HNFinanceTool.Basement;
using System.Xml;
using System.Data;

namespace CaoJin.HNFinanceTool.Dal
{
    //实现xml数据操作
    public class XmlOperate
    {
        private XmlOperate()
        { }
        public XmlOperate(string filepath)
        {
            _filePath = filepath;
        }

        private string _filePath;

        //创建xml文件
        public static void CreatXmlFile(string filepath)
        {
            XmlDocument doc = XmlHelper.CreateXmlDocument("finance", "");
            doc.Save(filepath);
        }

        //通过xml文件，读取到dataset
        public static DataSet GetDataSet(string filepath)
        {
            return XmlHelper.GetDataSet(filepath,XmlHelper.XmlType.File);
        }
        //插入数据
        public static void Insert(string filepath,string element,string value)
        {

        }
 
    }
}
