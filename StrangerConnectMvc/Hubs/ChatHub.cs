using Microsoft.AspNetCore.SignalR;
using StrangerConnectMvc.Models;

namespace StrangerConnectMvc.Hubs
{
    public class ChatHub : Hub
    {
        private static List<User> Users = new();

        public async Task JoinChat(string nickname)
        {
            var currentUser = new User
            {
                ConnectionId = Context.ConnectionId,
                NickName = nickname,
                IsAvailable = true
            };

            // Waiting user search
            var partner = Users.FirstOrDefault(u => u.IsAvailable);

            if (partner != null)
            {
                // Pair users
                partner.IsAvailable = false;
                currentUser.IsAvailable = false;

                partner.PartnerConnectionId = currentUser.ConnectionId;
                currentUser.PartnerConnectionId = partner.ConnectionId;

                Users.Add(currentUser);

                await Clients.Client(partner.ConnectionId)
                    .SendAsync("Matched", currentUser.NickName);

                await Clients.Client(currentUser.ConnectionId)
                    .SendAsync("Matched", partner.NickName);
            }
            else
            {
                Users.Add(currentUser);
                await Clients.Caller.SendAsync("Waiting");
            }
        }

        public async Task SendMessage(string message)
        {
            var sender = Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (sender?.PartnerConnectionId != null)
            {
                await Clients.Client(sender.PartnerConnectionId)
                    .SendAsync("ReceiveMessage", sender.NickName, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

            if (user != null)
            {
                var partner = Users.FirstOrDefault(u =>
                    u.ConnectionId == user.PartnerConnectionId);

                if (partner != null)
                {
                    partner.IsAvailable = true;
                    partner.PartnerConnectionId = null;

                    await Clients.Client(partner.ConnectionId)
                        .SendAsync("PartnerLeft");
                }

                Users.Remove(user);
            }

            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendOffer(string offer)
        {
            var user = Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user?.PartnerConnectionId != null)
            {
                await Clients.Client(user.PartnerConnectionId)
                    .SendAsync("ReceiveOffer", offer);
            }
        }

        public async Task SendAnswer(string answer)
        {
            var user = Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user?.PartnerConnectionId != null)
            {
                await Clients.Client(user.PartnerConnectionId)
                    .SendAsync("ReceiveAnswer", answer);
            }
        }

        public async Task SendIceCandidate(string candidate)
        {
            var user = Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user?.PartnerConnectionId != null)
            {
                await Clients.Client(user.PartnerConnectionId)
                    .SendAsync("ReceiveIceCandidate", candidate);
            }
        }

    }
}
