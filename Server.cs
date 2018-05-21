using System;

public class Server
{
        private string host;
        private int port;
        private bool stream_enable;
        private const int CHUNK_SIZE = 8;

        public Client(string host, int port, bool stream_enable)
        {
            this.host = host;
            this.port = port;
            this.stream_enable = stream_enable;
        }

      


    }
}
