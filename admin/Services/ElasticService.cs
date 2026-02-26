using Nest;
using System;

public class ElasticService
{
    private readonly ElasticClient _client;

    public ElasticService()
    {
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("livres");

        _client = new ElasticClient(settings);
    }

    public ElasticClient GetClient()
    {
        return _client;
    }
}
