namespace TrelloNet.Domain
{
    public class Data : Entity
    {

        public Card Card { get; set; }
        public Board Board { get; set; }
        public Old Old { get; set; }
        public List List { get; set; }
        public CheckItem CheckItem { get; set; }
        public string Text { get; set; }
        public string Embedly { get; set; }
        public Attachment Attachment { get; set; }
    }

    public class Attachment : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class CheckItem :Entity
    {
        public string Name { get; set; }
        
        //TODO: Make this an enum.
        public string State { get; set; }
    }

    public class List : Entity
    {
        public string Name { get; set; }
    }

    public class Old
    {
        public string Desc { get; set; }
        public string Name { get; set; }
    }
}