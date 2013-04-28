
using System.Collections;
using System.Collections.Generic;
namespace RuneSlinger.server.Abstract
{
    public interface IApplication
    {
        Registry Registry { get; }
        IEnumerable<INetworkedSession> Sessions { get; }
    }
}
