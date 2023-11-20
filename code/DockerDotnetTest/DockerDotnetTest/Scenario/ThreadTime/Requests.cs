using System.Net.Http;

namespace DockerDotnetTest.Scenario.ThreadTime;

public class Requests
{
    private static List<string> Urls = new List<string>
    {
        "https://www.computerbild.de/rss/35034859.xml",
        "https://www.finanznachrichten.de/rss-aktien-nachrichten",
        "https://www.tagesschau.de/infoservices/alle-meldungen-100~rdf.xml",
        "https://www.spiegel.de/schlagzeilen/index.rss",
        "https://www.heise.de/rss/heise.rdf",
        "https://rp-online.de/nrw/staedte/kempen/feed.rss",
        "https://www.faz.net/rss/aktuell/"
    };

    public List<string> GetFeeds()
    {
        var strings = new List<string>();

        for (int i = 0; i < Urls.Count; i++)
        {
            strings.Add(GetFeed(Urls[i]));
        }

        return strings;
    }

    public async Task<List<string>> GetFeedsAsync()
    {
        var tasks = new List<Task<string>>();
        var strings = new List<string>();

        for (int i = 0; i < Urls.Count; i++)
        {
            tasks.Add(GetFeedAsync(Urls[i]));
        }

        while (tasks.Count > 0)
        {
            var resolved = await Task.WhenAny(tasks);
            tasks.Remove(resolved);
            strings.Add(resolved.Result);
        }

        return strings;
    }

    public async Task<string> GetFeedAsync(string address)
    {
        using var httpClient = new HttpClient();
        using HttpResponseMessage response = await httpClient.GetAsync(address);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{address} success\n");
        return jsonResponse ?? string.Empty;
    }

    public string GetFeed(string address)
    {
        using var httpClient = new HttpClient();
        using HttpResponseMessage response = httpClient.GetAsync(address).Result;
        var jsonResponse = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine($"{address} success\n");
        return jsonResponse ?? string.Empty;
    }
}