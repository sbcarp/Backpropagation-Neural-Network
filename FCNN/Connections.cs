using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCNN
{
    // Connections class manage all connections in the network
    class Connections
    {
        public Connections()
        {
            ConnectionList = new List<Connection>();
        }

        public List<Connection> ConnectionList { get; set; }

    }
}
