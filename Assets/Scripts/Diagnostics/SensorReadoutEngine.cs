using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensorReadoutEngine : MonoBehaviour
{
    [Header("UI Target")]
    [SerializeField] private TMP_Text readoutText;

    [Header("Sensor State")]
    private string gyroStatus = "Not checked";
    private string accelStatus = "Not checked";
    private string compassStatus = "Not checked";
    private string locationStatus = "Not checked";

    private void Start()
    {
        if (readoutText == null)
        {
            enabled = false;
            return;
        }

        // Enable sensors
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            gyroStatus = "Enabled";
        }

        if (SystemInfo.supportsLocationService)
        {
            Input.location.Start();
            locationStatus = "Starting...";
        }

        if (SystemInfo.supportsVibration)
        {
            // Compass is implicitly started with location, but we check separately
            Input.compass.enabled = true;
        }
    }

    private void Update()
    {
        if (readoutText == null) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // ── System Info ──
        sb.AppendLine("<b>── System Info ──</b>");
        sb.AppendLine($"Gyroscope supported: {SystemInfo.supportsGyroscope}");
        sb.AppendLine($"Accelerometer supported: {SystemInfo.supportsAccelerometer}");
        sb.AppendLine($"Location supported: {SystemInfo.supportsLocationService}");
        sb.AppendLine($"Vibration supported: {SystemInfo.supportsVibration}");
        sb.AppendLine();

        // ── Accelerometer ──
        sb.AppendLine("<b>── Accelerometer ──</b>");
        Vector3 acc = Input.acceleration;
        sb.AppendLine($"Raw: ({acc.x:F4}, {acc.y:F4}, {acc.z:F4})");
        sb.AppendLine($"Magnitude: {acc.magnitude:F4} g");
        sb.AppendLine($"X tilt (jetpack angle): {acc.x * -45.0f:F2}°");
        sb.AppendLine();

        // ── Gyroscope ──
        sb.AppendLine("<b>── Gyroscope ──</b>");
        sb.AppendLine($"Supported: {SystemInfo.supportsGyroscope}");

        if (SystemInfo.supportsGyroscope)
        {
            Gyroscope g = Input.gyro;
            sb.AppendLine($"Enabled: {g.enabled}");
            sb.AppendLine($"Update Interval: {g.updateInterval}");
            sb.AppendLine($"Attitude: {g.attitude}");
            sb.AppendLine($"Rotation Rate: ({g.rotationRate.x:F4}, {g.rotationRate.y:F4}, {g.rotationRate.z:F4})");
            sb.AppendLine($"Gravity: ({g.gravity.x:F4}, {g.gravity.y:F4}, {g.gravity.z:F4})");
            sb.AppendLine($"User Acceleration: ({g.userAcceleration.x:F4}, {g.userAcceleration.y:F4}, {g.userAcceleration.z:F4})");
            sb.AppendLine($"Gravity X (reported roll): {g.gravity.x * -45.0f:F2}°");
        }
        else
        {
            sb.AppendLine("<color=red>No gyroscope available</color>");
        }
        sb.AppendLine();

        // ── Compass (Magnetometer) ──
        sb.AppendLine("<b>── Compass ──</b>");

        if (SystemInfo.supportsLocationService)
        {
            sb.AppendLine($"Enabled: {Input.compass.enabled}");
            sb.AppendLine($"Magnetic Heading: {Input.compass.magneticHeading:F2}°");
            sb.AppendLine($"True Heading: {Input.compass.trueHeading:F2}°");
            sb.AppendLine($"Heading Accuracy: {Input.compass.headingAccuracy:F2}°");
            sb.AppendLine($"Raw Vector: {Input.compass.rawVector}");
        }
        else
        {
            sb.AppendLine("<color=red>No compass/location available</color>");
        }
        sb.AppendLine();

        // ── Location (GPS) ──
        sb.AppendLine("<b>── Location ──</b>");

        if (SystemInfo.supportsLocationService)
        {
            LocationServiceStatus locStatus = Input.location.status;
            sb.AppendLine($"Status: {locStatus}");

            if (locStatus == LocationServiceStatus.Running)
            {
                LocationInfo li = Input.location.lastData;
                sb.AppendLine($"Latitude: {li.latitude:F6}");
                sb.AppendLine($"Longitude: {li.longitude:F6}");
                sb.AppendLine($"Altitude: {li.altitude:F2} m");
                sb.AppendLine($"Horizontal Accuracy: {li.horizontalAccuracy:F2} m");
                sb.AppendLine($"Vertical Accuracy: {li.verticalAccuracy:F2} m");
                sb.AppendLine($"Timestamp: {li.timestamp}");
            }
        }
        else
        {
            sb.AppendLine("<color=red>No location services available</color>");
        }
        sb.AppendLine();

        // ── Touch ──
        sb.AppendLine("<b>── Touch ──</b>");
        sb.AppendLine($"Touch Count: {Input.touchCount}");

        int maxTouchesToShow = Mathf.Min(Input.touchCount, 6);
        for (int i = 0; i < maxTouchesToShow; i++)
        {
            Touch t = Input.GetTouch(i);
            sb.AppendLine($"  [{t.fingerId}] Phase: {t.phase}  Pos: ({t.position.x:F0}, {t.position.y:F0})");
            sb.AppendLine($"         Delta: ({t.deltaPosition.x:F1}, {t.deltaPosition.y:F1})  Pressure: {t.pressure:F3}");
        }
        sb.AppendLine();

        readoutText.text = sb.ToString();
    }

    private void OnDestroy()
    {
        // Clean up
        if (SystemInfo.supportsLocationService && Input.location.isEnabledByUser)
        {
            Input.location.Stop();
        }
    }
}
