using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace qcre
{
    struct QCModel
    {
        public string ModelName;
        public QCBodyGroup[] BodyGroups;
        public QCSequence[] Sequences;
    }
    struct QCBodyGroup
    {
        public string Name;
        public string[] Models;
    }
    struct QCSequence
    {
        public string Name;
        public string Smd;
        public bool Loop;
        public int Fps;
    }
    struct QCCollisionModel
    {
        public string Smd;
    }
    class QCParser
    {
        public QCModel Parse(string path)
        {
            var cur = new QCModel();
            var bodyGroups = new List<QCBodyGroup>();

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    var lineGroup = line.Split(' ');

                    if (line.StartsWith("//"))  //Skip comment
                        continue;
                    else if (lineGroup[0] == "$modelname")
                        cur.ModelName = lineGroup[1].Trim('\"');
                    else if (lineGroup[0] == "$bodygroup")
                        bodyGroups.Add(ParseBodyGroup(sr, lineGroup[1].Trim('\"')));
                }
            }
            cur.BodyGroups = bodyGroups.ToArray();
            return cur;
        }
        QCBodyGroup ParseBodyGroup(StreamReader sr, string bodyGroupName)
        {
            var cur = new QCBodyGroup();
            var models = new List<string>();
            cur.Name = bodyGroupName;

            string line;
            while ((line = sr.ReadLine().Trim()) != "}")
            {
                var lineGroup = line.Split(' ');

                if (lineGroup[0] == "studio")
                    models.Add(lineGroup[1].Trim('\"'));
                else if (lineGroup[0] == "blank")
                    models.Add("Blank");
            }

            cur.Models = models.ToArray();
            return cur;
        }
    }
}
