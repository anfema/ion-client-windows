namespace Anfema.Ion.DataModel
{
    public class DataTypeModel
    {        
        public DataTypeModel(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public string name { get; set; }

        public string path { get; set; }
    }
}