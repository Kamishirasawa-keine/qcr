#pragma warning disable CS8600
#pragma warning disable CS8602
using System.IO;

namespace qcre
{
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
                        case "$attachment":
                            ParseAttachment(tokens);
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
                switch (tokens[0])
                {
                    case "studio":
                        models.Add(tokens[1].Trim('\"'));
                        break;
                    case "blank":
                        models.Add("Blank");
                        break;
                    default:
                        throw new Exception($"Unknown token: {tokens[0]}");
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
                switch (tokens[0])
                {
                    case "is_flexible":
                        ParseJiggleFlexible();
                        break;
                    case "is_rigid":    //TODO
                        break;
                    case "has_base_spring":
                        ParseJiggleBaseSpring();
                        break;
                    case "is_boing":
                        ParseJiggleBoing();
                        break;
                    default:
                        throw new Exception($"Unknown token: {tokens[0]}");
                }
            });

            return jigglebone;
        }
        QCJiggleFlexible ParseJiggleFlexible()
        {
            var flexible = new QCJiggleFlexible();

            ProcessBlock(line =>
            {
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
                        throw new Exception($"Unknown token: {tokens[0]}");
                }
            });

            return flexible;
        }
        QCJiggleBaseSpring ParseJiggleBaseSpring()
        {
            var baseSpring = new QCJiggleBaseSpring();

            ProcessBlock(line =>
            {
                switch (tokens[0])
                {
                    default:
                        throw new Exception($"Unknown token: {tokens[0]}");
                }
            });

            return baseSpring;
        }
        QCJiggleBoing ParseJiggleBoing()
        {
            var boing = new QCJiggleBoing();

            ProcessBlock(line =>
            {
                switch (tokens[0])
                {
                    default:
                        throw new Exception($"Unknown token: {tokens[0]}");
                }
            });

            return boing;
        }
        QCAttachment ParseAttachment(string[] tokens)
        {
            //$attachment(0) "attachmentName"(1) "boneName"(2) x(3) y(4) z(5) option(6) x(7) y(8) z(9)
            var attachment = new QCAttachment()
            {
                attachmentName = tokens[1].Trim('\"'),
                boneName = tokens[2].Trim('\"'),
                position = new System.Numerics.Vector3(float.Parse(tokens[3]), float.Parse(tokens[4]), float.Parse(tokens[5]))
            };

            switch (tokens[6])
            {
                case "absolute":
                case "rigid":
                case "world_align":
                case "rotate":
                case "x_and_z_axes":
                    break;
                default:
                    throw new Exception($"Unknown attachment option: {tokens[6]}");
            }

            return attachment;
        }
        private void ProcessBlock(Action<string> action)
        {
            tokens = null;
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
                        tokens = line.Split(' ');
                        action(line);
                        break;
                }
            }
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static bool ShouldSkip(string line) => line.StartsWith("//") || line == "";     //Skip comment and empty line
        private StreamReader? streamReader = null;
        private string[]? tokens = null;
    }
}
