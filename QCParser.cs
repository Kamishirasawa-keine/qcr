#pragma warning disable CS8600
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace qcre
{
    struct QCModel
    {
        public string modelName;
    }
    struct QCJiggleBone
    {
        public string boneName;
        public int boneIndex;
    }
    struct QCBodyGroup
    {
        public string bodyGroupName;
        public string[] models;
    }
    class QCParser
    {
        public QCModel Parse(string path)
        {
            var model = new QCModel();
            streamReader = new StreamReader(path);

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                line = line.Trim();
                if (ShouldSkip(line))
                    continue;
                var tokens = line.Split(' ');

                switch (tokens[0])
                {
                    case "$modelname":
                        model.modelName = tokens[1].Trim('\"');
                        break;
                    case "$bodygroup":
                        ParseBodyGroup(tokens[1].Trim('\"'));
                        break;
                    case "$jigglebone":
                        ParseJiggleBone(tokens[1].Trim('\"'));
                        break;
                    default:
                        throw new Exception($"Unknown token: {tokens[0]}");
                }
            }
            return model;
        }
        QCBodyGroup ParseBodyGroup(string bodyGroupName)
        {
            var bodyGroup = new QCBodyGroup()
            {
                bodyGroupName = bodyGroupName,
            };

            string line;
            bool isStarted = false;
            bool isEnded = false;
            var models = new List<string>();
            while ((line = streamReader.ReadLine()) != null && !isEnded)
            {
                line = line.Trim();
                if (ShouldSkip(line))
                    continue;
                var tokens = line.Split(' ');

                switch (tokens[0])
                {
                    case "{":
                        if (!isStarted)
                            isStarted = true;
                        else
                            throw new Exception("Duplicate open bracket");
                        break;
                    case "}":
                        if (!isStarted)
                            throw new Exception("Got an close bracket but there's no open bracket before it");
                        isEnded = true;
                        break;
                    case "studio":
                        models.Add(tokens[1].Trim('\"'));
                        break;
                    case "blank":
                        models.Add("Blank");
                        break;
                    default:
                        throw new Exception($"Unknown token in body group: {tokens[0]}");
                }
            }

            bodyGroup.models = [.. models];
            return bodyGroup;
        }
        QCJiggleBone ParseJiggleBone(string boneName)
        {
            var jigglebone = new QCJiggleBone()
            {
                boneName = boneName,
            };

            string line;
            bool isStarted = false;
            bool isEnded = false;
            while ((line = streamReader.ReadLine()) != null && !isEnded)
            {
                line = line.Trim();
                if (ShouldSkip(line))
                    continue;
                var tokens = line.Split(' ');

                switch (tokens[0])
                {
                    case "{":
                        if (!isStarted)
                            isStarted = true;
                        else
                            throw new Exception("Duplicate open bracket");
                        break;
                    case "}":
                        if (!isStarted)
                            throw new Exception("Got an close bracket but there's no open bracket before it");
                        isEnded = true;
                        break;
                    case "is_flexible":
                        ParseJiggleFlexible();
                        break;
                    case "is_rigid":    //TODO
                        break;
                    case "has_base_spring": //TODO
                        break;
                    case "is_boing":    //TODO
                        break;
                    default:
                        throw new Exception($"Unknown token in jigglebone: {tokens[0]}");
                }
            }
            return jigglebone;
        }
        void ParseJiggleFlexible()
        {

        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool ShouldSkip(string line)
        {
            return line.StartsWith("//") || line == "";     //Skip comment and empty line
        }
        private StreamReader streamReader;
    }
}
