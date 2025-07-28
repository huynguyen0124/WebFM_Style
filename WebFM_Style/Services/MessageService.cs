
namespace WebBanSon.Service
{
    public class MessageService
    {


        public interface IUserConnectionService
        {
            void AddConnection(string userId, string connectionId);
            void RemoveConnection(string connectionId);
            string GetUserByConnectionId(string connectionId);
            string GetConnectionId(string userId); // Add this method
        }

        // UserConnectionService.cs
        public class UserConnectionService : IUserConnectionService
        {
            private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

            public void AddConnection(string userId, string connectionId)
            {
                _connections[userId] = connectionId;
            }

            public void RemoveConnection(string connectionId)
            {
                var userIdToRemove = _connections.FirstOrDefault(x => x.Value == connectionId).Key;
                if (userIdToRemove != null)
                {
                    _connections.Remove(userIdToRemove);
                }
            }

            public string GetUserByConnectionId(string connectionId)
            {
                return _connections.FirstOrDefault(x => x.Value == connectionId).Key;
            }
            public string GetConnectionId(string userId)
            {
                return _connections.TryGetValue(userId, out var connectionId) ? connectionId : null;
            }
        }
    }
}
