using MetX.Library;
using System.Collections.Generic;
using System.IO;

namespace NpcsToCards
{
    public class NpcReader
    {
        private StreamReader _file;

        public List<Npc> Npcs;

        public NpcReader(StreamReader file)
        {
            if (file != null && !file.EndOfStream)
                _file = file;
        }

        public void ReadAll()
        {
            Npcs = new List<Npc>();
            Npc npc;
            do
            {
                npc = ReadNext();
                if (npc != null)
                    Npcs.Add(npc);
            } while (npc != null);
        }

        public Npc ReadNext()
        {
            if (_file == null)
                return null;

            if (_file.EndOfStream)
            {
                _file.Close();
                _file.Dispose();
                _file = null;
                return null;
            }

            var npcId = -1;
            while (npcId == -1 && !_file.EndOfStream)
            {
                var line = _file.ReadLine();
                var word = line.FirstToken(" ");
                if (int.TryParse(word, out npcId))
                {
                    var npcName = _file.ReadLine();
                    var title = _file.ReadLine();
                    var quoteFromNpc = _file.ReadLine();
                    var appearance = _file.ReadLine();
                    var roleplaying = _file.ReadLine();
                    var personality = _file.ReadLine();
                    var motivation = _file.ReadLine();
                    var background = _file.ReadLine();
                    var traits = _file.ReadLine();
                    return new Npc(npcId, npcName, title, quoteFromNpc, appearance,
                        roleplaying, personality, motivation, background,
                        traits);
                }
                npcId = -1;
            }
            return null;
        }
    }
}