using System.Collections.Generic;

namespace Kuiper.ServiceInfra.Security.Certificates;

public class JsonWebKeySet
{
    public List<JsonWebKey> Keys { get; set; } = new List<JsonWebKey>();
}