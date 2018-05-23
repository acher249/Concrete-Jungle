using UnityEngine;

namespace Leap.Unity
{
    /// <summary>
    /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture. 
    /// The component allows translation and scale of the object.

    /// Translate by pinching one hand.
    /// Scale by pinching both your hands.

    /// Objects can be selected by pointing at them.
    /// When pointing with your index finger, a lasser will appear. Use the laser to select objects. //TODO: let the laser appear green when an object was selected

    /// </summary>
    public class LeapMoveObjects : MonoBehaviour
    {

        [SerializeField]
        private PinchDetector _pinchDetectorA;

        public PinchDetector PinchDetectorA
        {
            get
            {
                return _pinchDetectorA;
            }
            set
            {
                _pinchDetectorA = value;
            }
        }

        [SerializeField]
        private PinchDetector _pinchDetectorB;

        public PinchDetector PinchDetectorB
        {
            get
            {
                return _pinchDetectorB;
            }
            set
            {
                _pinchDetectorB = value;
            }
        }

        [SerializeField]
        private ExtendedFingerDetector _extendedFinterDetectorL;

        public ExtendedFingerDetector ExtendedFingerDetectorL
        {
            get
            {
                return _extendedFinterDetectorL;
            }
            set
            {
                _extendedFinterDetectorL = value;
            }
        }

        [SerializeField]
        private ExtendedFingerDetector _extendedFinterDetectorR;

        public ExtendedFingerDetector ExtendedFingerDetectorR
        {
            get
            {
                return _extendedFinterDetectorR;
            }
            set
            {
                _extendedFinterDetectorR = value;
            }
        }

        private Finger.FingerType FingerName = Finger.FingerType.TYPE_INDEX;

        [Range(1, 2)]
        public float strength = 2.0f;

        public IHandModel HandModelL;
        public IHandModel HandModelR;
        public GameObject selectedObject;
        private GameObject hitObject;
        //if onTarget > ON_TARGET_THRESHOLD hitObject becomes the selectedObject
        private int onTarget = 0;
        private readonly int ON_TARGET_THRESHOLD = 50;
        //public LineRenderer lr;

        private float _defaultNearClip;
        private Transform _anchor;
        private bool _didUpdate;

        void Start()
        {
            GameObject pinchControl = new GameObject("RTS Anchor");
            _anchor = pinchControl.transform;
            _anchor.transform.parent = transform.parent;
        }

        void Update()
        {
            _didUpdate = false;
            if (_pinchDetectorA != null)
                _didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
            if (_pinchDetectorB != null)
                _didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

            if (_didUpdate)
            {
                if (selectedObject != null)
                {
                    selectedObject.transform.SetParent(null, true);
                }
            }

            if (_pinchDetectorA != null && _pinchDetectorA.IsActive &&
                _pinchDetectorB != null && _pinchDetectorB.IsActive)
            {
                transformDoubleAnchor();
            }
            else if (_pinchDetectorA != null && _pinchDetectorA.IsActive)
            {
                transformSingleAnchor(_pinchDetectorA);
            }
            else if (_pinchDetectorB != null && _pinchDetectorB.IsActive)
            {
                transformSingleAnchor(_pinchDetectorB);
            }
            else if (_extendedFinterDetectorL != null && _extendedFinterDetectorL.IsActive)
            {
                Hand hand;
                hand = HandModelL.GetLeapHand();
                shootRaycast(hand);
            }
            else if (_extendedFinterDetectorR != null && _extendedFinterDetectorR.IsActive)
            {
                Hand hand;
                hand = HandModelR.GetLeapHand();
                shootRaycast(hand);
            }
            else
            {
                stopShootingRaycast();
            }

            if (_didUpdate)
            {
                selectedObject.transform.parent = _anchor;
            }
        }

        public Transform getAnchor()
        {
            return selectedObject.transform;
        }

        public void shootRaycast(Hand hand)
        {
            int selectedFinger = selectedFingerOrdinal();

            Vector3 firePoint = hand.Fingers[selectedFinger].TipPosition.ToVector3();
            //lr.enabled = true;
            //lr.SetPosition(0, firePoint);
            //Vector3 fingerDirection = lr.transform.forward;
            Vector3 fingerDirection = hand.Fingers[selectedFinger].Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToVector3();
            RaycastHit hit;
            if (Physics.Raycast(firePoint, fingerDirection, out hit, 2000.0f))
            { //was something hit?
                fingerDirection = hit.point;
                if (hit.collider.tag == "interactable")
                {
                    if (hitObject != null && hitObject.Equals(hit.collider.gameObject))
                    {
                        onTarget += 1;
                    }
                    else
                    {
                        hitObject = hit.collider.gameObject;
                        onTarget = 0;
                    }
                }

                if (onTarget > ON_TARGET_THRESHOLD)
                {
                    selectedObject.transform.SetParent(null, true);
                    selectedObject = hitObject;
                }
            }

            //lr.SetPosition(1, fingerDirection);
        }

        private int selectedFingerOrdinal()
        {
            switch (FingerName)
            {
                case Finger.FingerType.TYPE_INDEX:
                    return 1;
                case Finger.FingerType.TYPE_MIDDLE:
                    return 2;
                case Finger.FingerType.TYPE_PINKY:
                    return 4;
                case Finger.FingerType.TYPE_RING:
                    return 3;
                case Finger.FingerType.TYPE_THUMB:
                    return 0;
                default:
                    return 1;
            }
        }

        public void stopShootingRaycast()
        {
            //lr.enabled = false;
        }

        private void transformDoubleAnchor()
        {
            Vector3 rememberAnchor = _anchor.position;
            _anchor.position = selectedObject.transform.position;
            //if the motion is to apprupt and fast, the moddel gets tossed arround
            //you could also use a Vector.one for this opperation, however tests have proofen it better to half the values
            _anchor.localScale = new Vector3(0.5f, 0.5f, 0.5f) * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
        }

        private void transformSingleAnchor(PinchDetector singlePinch)
        {
            Vector3 pinchPoint = singlePinch.Position * strength;
            pinchPoint.y = _anchor.position.y;

            _anchor.position = pinchPoint;
        }
    }
}