using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A 2-Dimensional coordinate system able to convert both Vector2 and Vector3 values into itself and likewise
/// </summary>
public class GridLocation
{
	private int x;
	public int X {
		get
		{
			return x;
		}
		set
		{
			x = value;
			if (assignedObject != null) 
			{
				assignedObject.transform.position = TranslateToVector3(this, assignedObject.transform.position.z);
			}
		}
	}
	private int y;
	public int Y {
		get
		{
			return y;
		}
		set
		{
			y = value;
			if (assignedObject != null) 
			{
				assignedObject.transform.position = TranslateToVector3(this, assignedObject.transform.position.z);
			}
		}
	}

	public static readonly float gridConversionRate = 1f;

	private GameObject assignedObject = null;

	private static GridLocation origin
	{
		get
		{
			return new GridLocation();
		}
	}

	public GridLocation()
	{
		x = 0;
		y = 0;
	}

	public GridLocation(Vector2 _location)
	{
		x = (int)Mathf.Round(_location.x / gridConversionRate);
		y = (int)Mathf.Round(_location.y / gridConversionRate);
	}

	public GridLocation(Vector3 _location)
	{
		x = (int)Mathf.Round(_location.x / gridConversionRate);
		y = (int)Mathf.Round(_location.y / gridConversionRate);
	}

	public GridLocation(int _x, int _y)
	{
		x = _x;
		y = _y;
	}

	public GridLocation(GameObject _assignedObject)
	{
		Vector3 _location = _assignedObject.transform.position;

		x = (int)Mathf.Round(_location.x / gridConversionRate);
		y = (int)Mathf.Round(_location.y / gridConversionRate);
		assignedObject = _assignedObject;
		Vector3 _newPos = TranslateToVector3(this, assignedObject.transform.position.z);
		assignedObject.transform.position = _newPos;
	}

	///<summary>Converts a GridLocation to a Vector2</summary>
	public static Vector2 TranslateToVector2(GridLocation _coordinates)
	{
		float _x = _coordinates.x * gridConversionRate;
		float _y = _coordinates.y * gridConversionRate;

		return new Vector2(_x, _y);
	}

	///<summary>Converts a GridLocation to a Vector3</summary>
	public static Vector3 TranslateToVector3(GridLocation _coordinates, float _zValue)
	{
		float _x = _coordinates.x * gridConversionRate;
		float _y = _coordinates.y * gridConversionRate;

		return new Vector3(_x, _y, _zValue);
	}

	///<summary>Identical to 'new GridLocation(Vector2)'</summary>
	public static GridLocation Convert(Vector2 _input)
	{
		return new GridLocation(_input);
	}

	///<summary>Identical to 'new GridLocation(Vector3)'</summary>
	public static GridLocation Convert(Vector3 _input)
	{
		return new GridLocation(_input);
	}


	///<summary>Attaches a GameObject to the GridLocation Location</summary>
	///<param name="_attachObject">The GameObject to Attach</param>
	///<param name="_moveObject">If true, moves the GameObject to the coordinates' location. If false, updates coordinates to the GameObject's positon (snaps object position to grid)</param>
	public void AttachObject(GameObject _attachObject, bool _moveObject = false)
	{
		if (assignedObject == null)
		{
			assignedObject = _attachObject;
			if (_moveObject)
			{
				_attachObject.transform.position = TranslateToVector3(this, _attachObject.transform.position.z);
			}
			else
			{
				Vector3 _pos = _attachObject.transform.position;
				x = (int)Mathf.Round(_pos.x / gridConversionRate);
				y = (int)Mathf.Round(_pos.y / gridConversionRate);
				_attachObject.transform.position = TranslateToVector3(this, _attachObject.transform.position.z);
			}
		}
		else
		{
			throw new Exception("A GameObject is already attached to this 'GridLocation' object!");
		}
	}

	///<summary>Detaches a GameObject from the GridLocation</summary>
	public void DetachObject()
	{
		if (assignedObject != null)
		{
			assignedObject = null;
		}
		else
		{
			throw new Exception("A GameObject is not attached to this 'GridLocation' object!");
		}
	}

	public void MoveBy(int _x, int _y)
	{
		x += _x;
		y += _y;
		if (assignedObject != null)
		{
			Vector3 _pos = TranslateToVector3(this, assignedObject.transform.position.z);
			assignedObject.transform.position = _pos;
		}
	}

	public void MoveBy(Vector3 _movement)
	{
		GridLocation movementGrid = Convert(_movement);
		x += movementGrid.X;
		y += movementGrid.Y;
		if (assignedObject != null)
		{
			Vector3 _pos = TranslateToVector3(this, assignedObject.transform.position.z);
			assignedObject.transform.position = _pos;
		}
	}

	public void MoveTo(int _x, int _y)
	{
		x = _x;
		y = _y;
		if (assignedObject != null)
		{
			Vector2 _tl = TranslateToVector2(this);
			assignedObject.transform.position = new Vector3(_tl.x, _tl.y, assignedObject.transform.position.z);
		}
	}

	public void MoveTo(GridLocation _targetLocation)
	{
		x = _targetLocation.x;
		y = _targetLocation.y;
		if (assignedObject != null)
		{
			Vector2 _tl = TranslateToVector2(this);
			assignedObject.transform.position = new Vector3(_tl.x, _tl.y, assignedObject.transform.position.z);
		}
	}

	public float DistanceTo(GridLocation _target)
	{
		float _xDistance = _target.X - x;
		float _yDistance = _target.Y - y;

		float _distance = Mathf.Abs(Mathf.Sqrt(Mathf.Round(_xDistance * _xDistance) + Mathf.Round(_yDistance * _yDistance)));
		return _distance;
	}

	public static float Distance(GridLocation _pointOne, GridLocation _pointTwo)
	{
		float _xDistance = _pointOne.X - _pointTwo.X;
		float _yDistance = _pointTwo.Y - _pointTwo.Y;

		float _distance = Mathf.Abs(Mathf.Sqrt(Mathf.Round(_xDistance * _xDistance) + Mathf.Round(_yDistance * _yDistance)));
		return _distance;
	}

	public static Vector2 Direction(GridLocation _source, GridLocation _target)
    {
		float xDistance = _source.X - _target.X;
		float yDistance = _source.Y - _target.Y;
		Vector2 output = new Vector2();

		if (xDistance > 0)
        {
			output.x = 1;
        }
		else if (xDistance < 0)
        {
			output.x = -1;
        }
		else
        {
			output.x = 0;
        }

		if (yDistance > 0)
		{
			output.y = 1;
		}
		else if (yDistance < 0)
		{
			output.y = -1;
		}
		else
		{
			output.y = 0;
		}

		return output;
	}

	/*public void TransitionTo(int _x, int _y)
	{

	}

	private IEnumerator Moving(int _byX, int _byY)
	{
		for (int i = 0, )
	}
	public void Interpolate(int _iX, int _iY)
	{
		float _targetX = (float)_iX;
		float _targetY = (float)_iY;
		float _x = (float)this.x;
		float _y = (float)this.y;
	}*/
}