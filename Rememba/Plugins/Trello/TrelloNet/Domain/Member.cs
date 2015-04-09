using System.Collections.Generic;

namespace TrelloNet.Domain
{
    public class Member : Entity
    {
        public Member()
        {
            Boards = new List<Board>();
            Cards = new List<Card>();
            Organizations  = new List<Organization>();
            Actions = new List<Action>();
        }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public string Bio { get; set; }
        public string Initials { get; set; }
        public List<Board> Boards { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<Card> Cards { get; set; }
        public List<Action> Actions { get; set; }
       
    }

    public class Organization : Entity
    {
        public Organization()
        {
            Members = new List<Member>();
        }


        public string Name { get; set; }
        public string Description { get; set; }
        public List<Member> Members { get; set; }
    }
}