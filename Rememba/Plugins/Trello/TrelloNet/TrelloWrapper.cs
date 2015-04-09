using TrelloNet.Actions;

namespace TrelloNet
{
    public static class TrelloWrapper
    {
        private static GetAction _getAction;
        private static CreateAction _createAction;
        private static UpdateAction _updateAction;
        private static DeleteAction _deleteAction;
        
        public static void Init(string token, string key)
        {
            ServiceManager.Init(token,key);
            _getAction = new GetAction();
            _createAction = new CreateAction();
            _updateAction = new UpdateAction();
            _deleteAction = new DeleteAction();
        }

        public static GetAction Get()
        {
            return _getAction;
        }

        public static CreateAction Create()
        {
            return _createAction;
        }

        public static UpdateAction Update()
        {
            return _updateAction;
        }

        public static DeleteAction Delete()
        {
            return _deleteAction;
        }


    }
}