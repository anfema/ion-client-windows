using Anfema.Amp.Parsing;

namespace Anfema.Amp.DataModel
{
    public class AmpOptionContent : AmpContent
    {
        private string _value;

        public override void init(ContentNodeRaw contentNode)
        {
            base.init(contentNode);

            _value = contentNode.value;
        }
        
        public string selectedOption
        {
            get { return _value; }
        }
    }
}