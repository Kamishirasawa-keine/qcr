#pragma warning disable CS8600
using System.IO;

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
    struct QCJiggleFlexible
    {
        public float yawStiffness;
        public float yawDamping;
        public float pitchStiffness;
        public float pitchDamping;
        public float alongStiffness;
        public float alongDamping;
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

            using (streamReader = new StreamReader(path))
            {
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
            }
            return model;
        }
        QCBodyGroup ParseBodyGroup(string bodyGroupName)
        {
            var bodyGroup = new QCBodyGroup()
            {
                bodyGroupName = bodyGroupName,
            };
            var models = new List<string>();

            ProcessBlock(line =>
            {
                var tokens = line.Split(' ');
                switch (tokens[0])
                {
                    case "studio":
                        models.Add(tokens[1].Trim('\"'));
                        break;
                    case "blank":
                        models.Add("Blank");
                        break;
                    default:
                        throw new Exception($"Unknown token in body group: {tokens[0]}");
                }
            });

            bodyGroup.models = [.. models];
            return bodyGroup;
        }
        QCJiggleBone ParseJiggleBone(string boneName)
        {
            var jigglebone = new QCJiggleBone()
            {
                boneName = boneName,
            };

            ProcessBlock(line =>
            {
                var tokens = line.Split(' ');
                switch (tokens[0])
                {
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
            });

            return jigglebone;
        }
        QCJiggleFlexible ParseJiggleFlexible()
        {
            var flexible = new QCJiggleFlexible();

            ProcessBlock(line =>
            {
                var tokens = line.Split(' ');
                switch (tokens[0])
                {
                    case "yaw_stiffness":
                        flexible.yawStiffness = float.Parse(tokens[1]);
                        break;
                    case "yaw_damping":
                        flexible.yawDamping = float.Parse(tokens[1]);
                        break;
                    case "pitch_stiffness":
                        flexible.pitchStiffness = float.Parse(tokens[1]);
                        break;
                    case "pitch_damping":
                        flexible.pitchDamping = float.Parse(tokens[1]);
                        break;
                    case "along_stiffness":
                        flexible.alongStiffness = float.Parse(tokens[1]);
                        break;
                    case "along_damping":
                        flexible.alongDamping = float.Parse(tokens[1]);
                        break;
                    default:
                        throw new Exception($"Unknown token in jigglebone flexible: {tokens[0]}");
                }
            });

            return flexible;
        }
        private void ProcessBlock(Action<string> action)
        {
            bool isStarted = false;
            bool isEnded = false;

            while (!isEnded && (streamReader.ReadLine() is string line))
            {
                line = line.Trim();
                if (ShouldSkip(line))
                    continue;

                switch (line)
                {
                    case "{":
                        if (isStarted)
                            throw new Exception("Duplicated open bracket");
                        break;
                    case "}":
                        if (!isStarted)
                            throw new Exception("Close bracket without an open bracket");
                        isEnded = true;
                        break;
                    default:
                        action(line);
                        break;
                }
            }
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static bool ShouldSkip(string line) => line.StartsWith("//") || line == "";     //Skip comment and empty line
        private StreamReader? streamReader;
    }
}
