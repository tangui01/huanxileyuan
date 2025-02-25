using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class JoystickCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

        [Range(1, 100)]
        public int moveRangePercent = 5;
		 int MovementRange = 10;      //% with the screen width
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
		public Transform targetDotImage, targetRing;
		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
            MovementRange = Screen.width * moveRangePercent / 100;
            m_StartPos = targetDotImage.position;
			//targetDotImage.gameObject.SetActive (false);
			//targetRing.gameObject.SetActive (false);
        }

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}

			SuperCommandoControllerInput.Instance.Horizontak = -delta.x;
			SuperCommandoControllerInput.Instance.Vertical = delta.y;
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}

		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;

			if (m_UseX)
			{
				int delta = (int)(data.position.x - m_StartPos.x);
//				Debug.LogError (delta);
//				delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
				newPos.x = delta;
			}

			if (m_UseY)
			{
				int delta = (int)(data.position.y - m_StartPos.y);
//				delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
				newPos.y = delta;
			}

//			Debug.LogError (Vector3.ClampMagnitude (new Vector3 (newPos.x, newPos.y, newPos.z), MovementRange));
//			transform.position = new Vector3(m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z);
			targetDotImage.transform.position = Vector3.ClampMagnitude( new Vector3(newPos.x, newPos.y, newPos.z),MovementRange) + m_StartPos;
			UpdateVirtualAxes(targetDotImage.transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			targetDotImage.transform.position = m_StartPos;
            //
            UpdateVirtualAxes(m_StartPos);
			//targetDotImage.gameObject.SetActive (false);
			//targetRing.gameObject.SetActive (false);

		}

		


		public void OnPointerDown(PointerEventData data) {
			//m_StartPos = data.position;
			targetDotImage.transform.position = m_StartPos;
			targetRing.transform.position = m_StartPos;
			//targetDotImage.gameObject.SetActive (true);
			//targetRing.gameObject.SetActive (true);
		}

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
			UpdateVirtualAxes(m_StartPos);
			//targetDotImage.gameObject.SetActive(false);
			//targetRing.gameObject.SetActive(false);
		}
	}
}