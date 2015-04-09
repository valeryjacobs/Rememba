using System.Collections.Generic;

namespace TrelloNet.Domain
{
    public class Card : Entity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Url { get; set; }
        public string IdList { get; set; }
        public bool Closed { get; set; }
        public string IdShort { get; set; }

        //TODO: find a way to lazy load the members.
        //TODO: or something better than this.
        public List<string> IdMembers { get; set; }
    }
}