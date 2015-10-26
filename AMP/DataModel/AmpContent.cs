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
   

        public virtual void init( ContentNodeRaw contentNode )
        {
            _variation = contentNode.variation;
            _outlet = contentNode.outlet;
            _isSearchable = contentNode.is_searchable;
            _type = contentNode.type;
            //_position = contentNode.position;  TODO:  not implemented in Amp yet
        }
    }
}
