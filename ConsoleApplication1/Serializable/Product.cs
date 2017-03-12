using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


[Serializable]
public class Product : ObjectBase, ISerializable
{
    public Product()
    {

    }
    public int Id { get; set; }
    private string connString
    {
        get; set;
    }
    [XmlIgnore]
    [NonSerialized]
    public SqlConnection conn;
    public Product(string s)
    {
        connString = s;
    }
    public override string ToString()
    {
        return this.connString;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("str", connString);
        info.AddValue("name", base.Name);
    }
    public Product(SerializationInfo info, StreamingContext context)
    {
        connString = info.GetString("str");
        base.Name = info.GetString("name");
    }
}
