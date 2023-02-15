using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomeDev.Planes
{
    public class AimIndicator : MonoBehaviour
    {
        public static AimIndicator instance = null;

        [SerializeField]
        float maxAngle = 90f;

        [SerializeField]
        public float maxPosition = 209f;

        public float levelPitchAngle = 0f;

        public float levelRollAngle = 0f;

        public float pointVerticalAngle = 0f;

        public float pointHorizontalAngle = 0f;


        RectTransform levelRect = null;

        RectTransform pointRect = null;

        public float altude = 0f;

        public float speed = 0f;

        public float acceleration = 0f;

        public float verticalSpeed = 0f;

        Text textAltude = null;

        Text textSpeed = null;

        Text textAcceleration = null;

        Text textVerticalSpeed = null;

        public void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            levelRect = transform.Find("Aim/Mask/Level").GetComponent<RectTransform>();
            pointRect = transform.Find("Aim/Mask/Point").GetComponent<RectTransform>();

            textAltude = transform.Find("AltudeText").GetComponent<Text>();
            textSpeed = transform.Find("SpeedText").GetComponent<Text>();
            textAcceleration = transform.Find("Acceleration").GetComponent<Text>();
            textVerticalSpeed = transform.Find("VerticalSpeed").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            DrawSymbols();

            WriteTexts();
        }

        private void DrawSymbols()
        {
            float curLevelPitchAngle = levelPitchAngle;

            if (curLevelPitchAngle > maxAngle)
            {
                curLevelPitchAngle = maxAngle;
            }
            else if (curLevelPitchAngle < -maxAngle)
            {
                curLevelPitchAngle = -maxAngle;
            }

            Vector2 levelOffset = new Vector2(0, ((curLevelPitchAngle / maxAngle) * maxPosition) - (levelRect.rect.height / 2f));

            Quaternion localRotation = Quaternion.Euler(0, 0, levelRollAngle);

            levelRect.localPosition = localRotation * levelOffset;

            levelRect.localRotation = localRotation;

            Vector2 pointPosition = new Vector2((pointHorizontalAngle / maxAngle) * maxPosition, (pointVerticalAngle / maxAngle) * maxPosition);

            pointRect.localPosition = localRotation * pointPosition;
        }

        private void WriteTexts()
        {
            textAltude.text = altude.ToString("0") + "m";

            textSpeed.text = speed.ToString("0") + "m/s";

            textAcceleration.text = acceleration.ToString("0.0") + "m/s*s";

            textVerticalSpeed.text = (verticalSpeed > 0 ? "+" : "") + verticalSpeed.ToString("0.0") + "m/s";
        }
    }
}