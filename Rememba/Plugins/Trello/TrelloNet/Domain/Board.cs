using System.Collections.Generic;


namespace TrelloNet.Domain
{
    public class Board : Entity
    {
        public string Name { get; set; }

        public List<Action> Actions { get; set; }
        //public string IdMemberCreator { get; set; }
        //public IEnumerable<CardData> Data { get; set; }
        //public string Type { get; set; }
        //public DateTime Date { get; set; }
    }
}