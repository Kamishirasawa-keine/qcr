using System.Numerics;

namespace qcre
{
    struct QCModel
    {
        public string modelName;
    }
    enum QCAttachmentAlign
    {
        Absolute,
        Rigid,
        WorldAlign,
        XAndZ
    }
    struct QCAttachment
    {
        public string attachmentName;
        public string boneName;
        public Vector3 position;
        public QCAttachmentAlign align;
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
    struct QCJiggleBaseSpring
    {
        public float mass;
        public float stiffness;
        public float damping;
        public float minLeft;
        public float maxLeft;
        public float leftFriction;
        public float minUp;
        public float maxUp;
        public float upFriction;
        public float minForward;
        public float maxForward;
        public float forwardFriction;
    }
    struct QCJiggleBoing
    {
        public float impactSpeed;
        public float impactAngle;
        public float dampingRate;
        public float frequency;
        public float amplitude;
    }
    struct QCBodyGroup
    {
        public string bodyGroupName;
        public string[] models;
    }
}