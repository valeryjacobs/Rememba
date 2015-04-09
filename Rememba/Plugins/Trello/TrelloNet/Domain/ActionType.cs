namespace TrelloNet.Domain
{
    public enum ActionType
    {
        CreateCard
        ,CommentCard
        ,UpdateCard
        ,UpdateCardidList //WAT?
        ,UpdateCardclosed //WAT?
        ,UpdateCarddesc
        ,UpdateCardname
        ,AddMemberToCard
        ,RemoveMemberFromCard
        ,UpdateCheckItemStateOnCard
        ,AddAttachmentToCard
        ,RemoveAttachmentFromCard
        ,AddChecklistToCard
        ,RemoveChecklistFromCard
        ,CreateList
        ,UpdateList
        ,CreateBoard
        ,UpdateBoard
        ,AddMemberToBoard
        ,RemoveMemberFromBoard
        ,AddToOrganizationBoard
        ,RemoveFromOrganizationBoard
        ,CreateOrganization
        ,UpdateOrganization
    }
}