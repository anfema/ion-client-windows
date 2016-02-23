using Anfema.Amp.Parsing;

namespace Anfema.Amp.DataModel
{
    /// <summary>
    /// Content base class that all content classes should inheritate from
    /// </summary> 
    public class AmpContent
    {
        private string _variation;
        private string _outlet;
        private bool _isSearchable;
        private int _position;
        private string _type;


        public string variation { get; set; }
        public string outlet { get; set; }
        public bool isSearchable { get; set; }
        public int position { get; set; }
        public string type { get; set; }

        public virtual void init( ContentNodeRaw contentNode )
        {
            variation = contentNode.variation;
            outlet = contentNode.outlet;
            isSearchable = contentNode.is_searchable;
            type = contentNode.type;
            //_position = contentNode.position;  TODO:  not implemented in Amp yet
        }
    }
}
