namespace Kuiper.ServiceInfra.Configuration;

using System;
using System.Collections.Generic;
using System.Text.Json;

internal class ConnectionSettings
{
    private readonly Dictionary<string, string> _parameters;

    public static ConnectionSettings Parse(string connectionString)
    {
        return new ConnectionSettings(connectionString);
    }

    public static T ParseAs<T>(string connectionString)
        where T : class, new()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyFields = false,
            IgnoreReadOnlyProperties = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var connectionSettings = Parse(connectionString);
        var parameters = JsonSerializer.Serialize(connectionSettings.GetAllParameters(), jsonOptions);

        return JsonSerializer.Deserialize<T>(parameters, jsonOptions)!;
    }

    public ConnectionSettings(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }

        _parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        this.ParseConnectionString(connectionString);
    }

    private void ParseConnectionString(string connectionString)
    {
        // Split the connection string by semicolon
        string[] parameters = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (string parameter in parameters)
        {
            string[] keyValuePair = parameter.Split('=', 2);

            if (keyValuePair.Length != 2)
            {
                throw new FormatException($"Invalid connection string format: '{parameter}'.");
            }

            string key = keyValuePair[0].Trim();
            string value = keyValuePair[1].Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new FormatException($"Connection string contains an empty key: '{parameter}'.");
            }

            // Add to dictionary (case-insensitive)
            _parameters[key] = value;
        }
    }

    public string GetParameterValue(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }

        // Retrieve value in a case-insensitive manner
        return _parameters.TryGetValue(key, out var value) ? value : null;
    }

    public IReadOnlyDictionary<string, string> GetAllParameters()
    {
        return _parameters;
    }
}