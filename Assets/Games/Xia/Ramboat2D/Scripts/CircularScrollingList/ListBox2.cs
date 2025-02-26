﻿/* The basic component of scrolling list.
 * Control the position and the contents of the list element.
 *
 * Author: LanKuDot <airlanser@gmail.com>
 */
using UnityEngine;
using UnityEngine.UI;

public class ListBox2 : MonoBehaviour
{
	public int listBoxID;	// Must be unique, and count from 0
	//public Text content;		// The content of the list box

	public ListBox2 lastListBox2;
	public ListBox2 nextListBox2;

	private int _contentID;

	// All position calculations here are in the local space of the list
	private Vector2 _canvasMaxPos;
	private Vector2 _unitPos;
	private Vector2 _lowerBoundPos;
	private Vector2 _upperBoundPos;
	private Vector2 _rangeBoundPos;
	private Vector2 _shiftBoundPos;

	private Vector3 _slidingDistance;	// The sliding distance at each frame
	private Vector3 _slidingDistanceLeft;

	private Vector3 _originalLocalScale;

	private bool _keepSliding = false;
	private int _slidingFramesLeft;

	public bool keepSliding { set { _keepSliding = value; } }

	/* Notice: ListBox2 will initialize its variables from ListPositionCtrl2.
	 * Make sure that the execution order of script ListPositionCtrl2 is prior to
	 * ListBox2.
	 */
	void Start()
	{
		_canvasMaxPos = ListPositionCtrl2.Instance.canvasMaxPos_L;
		_unitPos = ListPositionCtrl2.Instance.unitPos_L;
		_lowerBoundPos = ListPositionCtrl2.Instance.lowerBoundPos_L;
		_upperBoundPos = ListPositionCtrl2.Instance.upperBoundPos_L;
		_rangeBoundPos = ListPositionCtrl2.Instance.rangeBoundPos_L;
		_shiftBoundPos = ListPositionCtrl2.Instance.shiftBoundPos_L;

		_originalLocalScale = transform.localScale;

		initialPosition( listBoxID );
		initialContent();
	}

	/* Initialize the content of ListBox2.
	 */
	void initialContent()
	{
		if (listBoxID == ListPositionCtrl2.Instance.listBoxes.Length / 2)
			_contentID = 0;
		else if (listBoxID < ListPositionCtrl2.Instance.listBoxes.Length / 2)
			_contentID = ListBank.Instance.getListLength() - (ListPositionCtrl2.Instance.listBoxes.Length / 2 - listBoxID);
		else
			_contentID = listBoxID - ListPositionCtrl2.Instance.listBoxes.Length / 2;

		while (_contentID < 0)
			_contentID += ListBank.Instance.getListLength();
		_contentID = _contentID % ListBank.Instance.getListLength();

		//updateContent( ListBank.Instance.getListContent( _contentID ) );
	}

//	void updateContent( string content )
//	{
//		this.content.text = content;
//	}

	/* Make the list box slide for delta x or y position.
	 */
	public void setSlidingDistance( Vector3 distance, int slidingFrames )
	{
		_keepSliding = true;
		_slidingFramesLeft = slidingFrames;

		_slidingDistanceLeft = distance;
		_slidingDistance = Vector3.Lerp( Vector3.zero, distance, ListPositionCtrl2.Instance.slidingFactor );
	}

	/* Move the listBox for world position unit.
	 * Move up when "up" is true, or else, move down.
	 */
	public void unitMove( int unit, bool up_right )
	{
		Vector2 deltaPos;

		if (up_right)
			deltaPos = _unitPos * (float)unit;
		else
			deltaPos = _unitPos * (float)unit * -1;

		switch (ListPositionCtrl2.Instance.direction) {
		case ListPositionCtrl2.Direction.VERTICAL:
			setSlidingDistance( new Vector3( 0.0f, deltaPos.y, 0.0f ), ListPositionCtrl2.Instance.slidingFrames );
			break;
		case ListPositionCtrl2.Direction.HORIZONTAL:
			setSlidingDistance( new Vector3( deltaPos.x, 0.0f, 0.0f ), ListPositionCtrl2.Instance.slidingFrames );
			break;
		}
	}

	void Update()
	{
		if (_keepSliding) {
			--_slidingFramesLeft;
			if (_slidingFramesLeft == 0) {
				_keepSliding = false;
				// At the last sliding frame, move to that position.
				// At free moving mode, this function is disabled.
				if (ListPositionCtrl2.Instance.alignToCenter ||
					ListPositionCtrl2.Instance.controlByButton) {
					updatePosition( _slidingDistanceLeft );
				}
				// FIXME: Due to compiler optimization?
				// When using condition listBoxID == 0, there have some boxes don't execute
				// the above code. (For other condition, like 1, 3, or 4, also has the same
				// problem. Only using 2 will work normally.)
				if (listBoxID == 2 &&
					ListPositionCtrl2.Instance.needToAlignToCenter)
					ListPositionCtrl2.Instance.alignToCenterSlide();
				return;
			}

			updatePosition( _slidingDistance );
			_slidingDistanceLeft -= _slidingDistance;
			_slidingDistance = Vector3.Lerp( Vector3.zero, _slidingDistanceLeft, ListPositionCtrl2.Instance.slidingFactor );
		}
	}

	/* Initialize the local position of the list box accroding to its ID.
	 */
	void initialPosition( int listBoxID )
	{
		switch (ListPositionCtrl2.Instance.direction) {
		case ListPositionCtrl2.Direction.VERTICAL:
			transform.localPosition = new Vector3( 0.0f,
				_unitPos.y * (float)( listBoxID * -1 + ListPositionCtrl2.Instance.listBoxes.Length / 2 ),
				0.0f );
			updateXPosition();
			break;
		case ListPositionCtrl2.Direction.HORIZONTAL:
			transform.localPosition = new Vector3( _unitPos.x* (float)( listBoxID - ListPositionCtrl2.Instance.listBoxes.Length / 2 ),
				0.0f, 0.0f );
			updateYPosition();
			break;
		}
	}

	/* Update the local position of ListBox2 accroding to the delta position at each frame.
	 * Note that the deltaPosition must be in local space.
	 */
	public void updatePosition( Vector3 deltaPosition_L )
	{
		switch (ListPositionCtrl2.Instance.direction) {
		case ListPositionCtrl2.Direction.VERTICAL:
			transform.localPosition += new Vector3( 0.0f, deltaPosition_L.y, 0.0f );
			updateXPosition();
			checkBoundaryY();
			break;
		case ListPositionCtrl2.Direction.HORIZONTAL:
			transform.localPosition += new Vector3( deltaPosition_L.x, 0.0f, 0.0f );
			updateYPosition();
			checkBoundaryX();
			break;
		}
	}

	/* Calculate the x position accroding to the y position.
	 * Formula: x = max_x * angularity * cos( radian controlled by y )
	 * radian = (y / upper_y) * pi / 2, so the range of radian is from pi/2 to 0 to -pi/2,
	 * and corresponding cosine value is from 0 to 1 to 0.
	 */
	void updateXPosition()
	{
		transform.localPosition = new Vector3(
			_canvasMaxPos.x * ListPositionCtrl2.Instance.angularity
			* Mathf.Cos( transform.localPosition.y / _upperBoundPos.y * Mathf.PI / 2.0f ),
			transform.localPosition.y, transform.localPosition.z );
		updateSize( _upperBoundPos.y, transform.localPosition.y );
	}

	/* Calculate the y position accroding to the x position.
	 */
	void updateYPosition()
	{
		transform.localPosition = new Vector3(
			transform.localPosition.x,
			_canvasMaxPos.y * ListPositionCtrl2.Instance.angularity
			* Mathf.Cos( transform.localPosition.x / _upperBoundPos.x * Mathf.PI / 2.0f ),
			transform.localPosition.z );
		updateSize( _upperBoundPos.x, transform.localPosition.x );
	}

	/* Check if the ListBox2 is beyond the upper or lower bound or not.
	 * If does, move the ListBox2 to the other side and update the content.
	 */
	void checkBoundaryY()
	{
		float beyondPosY_L = 0.0f;

		// Narrow the checking boundary in order to avoid the list swaying to one side
		if (transform.localPosition.y < _lowerBoundPos.y + _shiftBoundPos.y) {
			beyondPosY_L = ( _lowerBoundPos.y + _shiftBoundPos.y - transform.localPosition.y ) % _rangeBoundPos.y;
			transform.localPosition = new Vector3(
				transform.localPosition.x,
				_upperBoundPos.y + _shiftBoundPos.y - _unitPos.y - beyondPosY_L,
				transform.localPosition.z );
			updateToLastContent();
		} else if (transform.localPosition.y > _upperBoundPos.y - _shiftBoundPos.y) {
			beyondPosY_L = ( transform.localPosition.y - _upperBoundPos.y + _shiftBoundPos.y ) % _rangeBoundPos.y;
			transform.localPosition = new Vector3(
				transform.localPosition.x,
				_lowerBoundPos.y - _shiftBoundPos.y + _unitPos.y + beyondPosY_L,
				transform.localPosition.z );
			updateToNextContent();
		}

		updateXPosition();
	}

	void checkBoundaryX()
	{
		float beyondPosX_L = 0.0f;

		// Narrow the checking boundary in order to avoid the list swaying to one side
		if (transform.localPosition.x < _lowerBoundPos.x + _shiftBoundPos.x) {
			beyondPosX_L = (_lowerBoundPos.x + _shiftBoundPos.x - transform.localPosition.x) % _rangeBoundPos.x;
			transform.localPosition = new Vector3(
				_upperBoundPos.x + _shiftBoundPos.x - _unitPos.x - beyondPosX_L,
				transform.localPosition.y,
				transform.localPosition.z );
			updateToNextContent();
		} else if (transform.localPosition.x > _upperBoundPos.x - _shiftBoundPos.x) {
			beyondPosX_L = (transform.localPosition.x - _upperBoundPos.x + _shiftBoundPos.x) % _rangeBoundPos.x;
			transform.localPosition = new Vector3(
				_lowerBoundPos.x - _shiftBoundPos.x + _unitPos.x + beyondPosX_L,
				transform.localPosition.y,
				transform.localPosition.z );
			updateToLastContent();
		}

		updateYPosition();
	}

	/* Scale the size of listBox accroding to the position.
	 */
	void updateSize( float smallest_at, float target_value )
	{
		transform.localScale = _originalLocalScale *
			( 1.0f + ListPositionCtrl2.Instance.scaleFactor * Mathf.InverseLerp( smallest_at, 0.0f, Mathf.Abs( target_value )));
	}

	public int getCurrentContentID()
	{
		return _contentID;
	}

	/* Update to the last content of the next ListBox2
	 * when the ListBox2 appears at the top of camera.
	 */
	void updateToLastContent()
	{
		_contentID = nextListBox2.getCurrentContentID() - 1;
		_contentID = ( _contentID < 0 ) ? ListBank.Instance.getListLength() - 1 : _contentID;

	//	updateContent( ListBank.Instance.getListContent( _contentID ) );
	}

	/* Update to the next content of the last ListBox2
	 * when the ListBox2 appears at the bottom of camera.
	 */
	void updateToNextContent()
	{
		_contentID = lastListBox2.getCurrentContentID() + 1;
		_contentID = ( _contentID == ListBank.Instance.getListLength() ) ? 0 : _contentID;

		//updateContent( ListBank.Instance.getListContent( _contentID ) );
	}
}
