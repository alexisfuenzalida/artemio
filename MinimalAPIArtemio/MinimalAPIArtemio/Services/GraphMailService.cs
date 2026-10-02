using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MinimalAPIArtemio.Options;

namespace MinimalAPIArtemio.Services;

public class GraphMailService
{
    private readonly GraphOptions _options;

    public GraphMailService(IOptions<GraphOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendHtmlAsync(string to, string subject, string htmlBody)
    {
        var credential = new ClientSecretCredential(_options.TenantId, _options.ClientId, _options.ClientSecret);
        var graph = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = BodyType.Html,
                Content = htmlBody
            },
            ToRecipients = new List<Recipient>
            {
                new()
                {
                    EmailAddress = new EmailAddress { Address = to }
                }
            }
        };

        await graph.Users[_options.Sender].SendMail.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
        {
            Message = message,
            SaveToSentItems = true
        });
    }
}
