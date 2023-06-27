using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public float vMove;
	public bool jump;
	public bool sprint;
	public bool firePrimary;
	public bool fireSecondary;
	public bool interact;
	public bool inventory;

	[Header("Movement Settings")]
	public bool analogMovement;

	//[Header("Mouse Cursor Settings")]
	private static bool cursorLocked = true;
	private static bool cursorInputForLook = true;


	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}
	public void OnVerticalMove(InputValue value)
    {
		VerticalMoveInput(value.Get<float>());
    }

	public void OnLook(InputValue value)
	{
		if (cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
        else
        {
			LookInput(Vector2.zero);
        }
	}
	public void OnJump(InputValue value)
	{
		JumpInput(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}

	public void OnFirePrimary(InputValue value)
    {
		FirePrimaryInput(value.isPressed);
    }

	public void OnFireSecondary(InputValue value)
	{
		FireSecondaryInput(value.isPressed);
	}

	public void OnInteract(InputValue value)
	{
		InteractInput(value.isPressed);
	}

	public void OnInventory(InputValue value)
    {
		InventoryInput(value.isPressed);
    }

	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	}

	public void VerticalMoveInput(float newVerticalMoveDirection)
    {
		vMove = newVerticalMoveDirection;
    }

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}

	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}

	public void FirePrimaryInput(bool newFirePrimaryState)
    {
		firePrimary = newFirePrimaryState;
    }

	public void FireSecondaryInput(bool newFireSecondaryState)
    {
		fireSecondary = newFireSecondaryState;
    }

	public void InteractInput(bool newInteractState)
	{
		interact = newInteractState;
	}
	public void InventoryInput(bool newInventoryState)
    {
		inventory = newInventoryState;
    }

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	public static bool GetLockState()
    {
		return cursorLocked;
    }
	public static void ModifyCursorState(bool lockmode, bool inputForLook)
    {
		cursorLocked = lockmode;
		cursorInputForLook = inputForLook;

        SetCursorState(cursorLocked);
	}
	private static void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
